using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MFlix.HttpApi.Infrastructure.Middleware
{
    public sealed class ServerErrorHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ServerErrorHandler> _logger;
        private readonly IWebHostEnvironment _environment;

        public ServerErrorHandler(
            RequestDelegate next,
            ILogger<ServerErrorHandler> logger,
            IWebHostEnvironment environment)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        public Task InvokeAsync(HttpContext context)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));
            return HandleInvokeAsync(context);
        }

        private async Task HandleInvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context).ConfigureAwait(true);
            }
            catch (RpcException exception) when (exception.StatusCode == StatusCode.InvalidArgument)
            {
                _logger.LogWarning(exception.Message);
                context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                var problem = CreateValidationProblem(exception, context);
                await context.Response.WriteAsJsonAsync(problem).ConfigureAwait(true);
            }
            catch (RpcException exception) when (exception.StatusCode == StatusCode.NotFound)
            {
                _logger.LogWarning(exception.Message);
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                var problem = CreateNotFoundProblem(context);
                await context.Response.WriteAsJsonAsync(problem).ConfigureAwait(true);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception exception)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                _logger.LogError(exception, exception.Message);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                var problem = CreateInternalServerErrorProblem(exception, context);
                await context.Response.WriteAsJsonAsync(problem).ConfigureAwait(true);
            }
        }

        private static ValidationProblemDetails CreateValidationProblem(RpcException exception, HttpContext context)
        {
            var errors = exception
                .Trailers
                .ToValidationResult()
                .Errors
                .Select(failure => new KeyValuePair<string, string[]>(failure.PropertyName, new string[] { failure.ErrorMessage }))
                .ToList();

            var problem = new ValidationProblemDetails(new Dictionary<string, string[]>(errors))
            {
                Detail = string.Empty,
                Instance = context.Request?.Path.Value ?? string.Empty,
                Status = StatusCodes.Status422UnprocessableEntity,
                Title = "There were validation errors",
                Type = $"https://httpstatuses.com/{StatusCodes.Status422UnprocessableEntity}"
            };
            problem.Extensions.Add("traceId", context.TraceIdentifier);

            return problem;
        }

        private static ProblemDetails CreateNotFoundProblem(HttpContext context)
        {
            var problemDetails = new ProblemDetails()
            {
                Detail = string.Empty,
                Instance = context.Request?.Path.Value ?? string.Empty,
                Status = StatusCodes.Status404NotFound,
                Title = "Resource not found",
                Type = $"https://httpstatuses.com/{StatusCodes.Status404NotFound}"
            };

            problemDetails.Extensions.Add("traceId", context.TraceIdentifier);

            return problemDetails;
        }

        private ProblemDetails CreateInternalServerErrorProblem(Exception exception, HttpContext context)
        {
            var problemDetails = new ProblemDetails()
            {
                Instance = context.Request?.Path.Value ?? string.Empty,
                Status = StatusCodes.Status500InternalServerError,
                Type = $"https://httpstatuses.com/{StatusCodes.Status500InternalServerError}"
            };

            if (_environment.IsDevelopment())
            {
                problemDetails.Detail = exception.StackTrace;
                problemDetails.Title = exception.Message;
            }
            else
            {
                problemDetails.Detail = string.Empty;
                problemDetails.Title = "An unpected server fault occurred";
            }

            problemDetails.Extensions.Add("traceId", context.TraceIdentifier);

            return problemDetails;
        }
    }
}
