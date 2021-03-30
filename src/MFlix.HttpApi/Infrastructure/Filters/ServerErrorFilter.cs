using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Grpc.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RpcStatusCode = Grpc.Core.StatusCode;

namespace MFlix.HttpApi.Infrastructure.Filters
{
    public sealed class ServerErrorFilter : IActionFilter, IOrderedFilter
    {
        private readonly ILogger<ServerErrorFilter> _logger;
        private readonly IWebHostEnvironment _environment;

        public ServerErrorFilter(ILoggerFactory loggerFactory, IWebHostEnvironment environment)
        {
            if (loggerFactory is null)
                throw new ArgumentNullException(nameof(loggerFactory));

            _logger = loggerFactory.CreateLogger<ServerErrorFilter>();
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        public int Order => int.MaxValue - 10;

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));

            if (context.Exception is not null)
            {
                var result = context.Exception switch
                {
                    RpcException ex when (ex.StatusCode == RpcStatusCode.InvalidArgument) => HandleInvalidArgument(ex, context.HttpContext),
                    RpcException ex when (ex.StatusCode == RpcStatusCode.NotFound) => HandleNotFound(ex, context.HttpContext),
                    _ => HandleUnexpectedException(context.Exception, context.HttpContext),
                };
                result.ContentTypes.Add(MediaTypeNames.Application.Json);
                result.ContentTypes.Add(MediaTypeNames.Application.Xml);
                context.Result = result;
                context.ExceptionHandled = true;
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        private ObjectResult HandleInvalidArgument(RpcException exception, HttpContext context)
        {
            _logger.LogWarning(exception.Message);
            context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
            return new ObjectResult(CreateValidationProblem(exception, context));
        }

        private static ValidationProblemDetails CreateValidationProblem(RpcException exception, HttpContext context)
        {
            var errors = exception
                .Trailers
                .ToValidationResult()
                .Errors
                .Select(failure => new KeyValuePair<string, string[]>(
                    key: failure.PropertyName,
                    value: new string[] { failure.ErrorMessage }))
                .ToList();

            var problem = new ValidationProblemDetails(new Dictionary<string, string[]>(errors))
            {
                Detail = string.Empty,
                Instance = context.Request?.Path.Value ?? string.Empty,
                Status = StatusCodes.Status422UnprocessableEntity,
                Title = $"There are {errors.Count} validation errors",
                Type = $"https://httpstatuses.com/{StatusCodes.Status422UnprocessableEntity}"
            };
            problem.Extensions.Add("traceId", context.TraceIdentifier);
            return problem;
        }

        private ObjectResult HandleNotFound(RpcException exception, HttpContext context)
        {
            _logger.LogWarning(exception.Message);
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            return new ObjectResult(CreateNotFoundProblem(context));
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

        private ObjectResult HandleUnexpectedException(Exception? exception, HttpContext context)
        {
            if (exception is null)
            {
                _logger.LogError(StatusCodes.Status500InternalServerError, "An unknown server error occurred");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                return new ObjectResult(CreateUnknownErrorProblem(context));
            }
            else
            {
                _logger.LogError(StatusCodes.Status500InternalServerError, exception, exception.Message);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                return new ObjectResult(CreateKnownErrorProblem(exception, context));
            }
        }

        private static ProblemDetails CreateUnknownErrorProblem(HttpContext context)
        {
            var problemDetails = new ProblemDetails()
            {
                Detail = string.Empty,
                Instance = context.Request?.Path.Value ?? string.Empty,
                Status = StatusCodes.Status500InternalServerError,
                Title = "A server fault occurred",
                Type = $"https://httpstatuses.com/{StatusCodes.Status500InternalServerError}"
            };
            problemDetails.Extensions.Add("traceId", context.TraceIdentifier);
            return problemDetails;
        }

        private ProblemDetails CreateKnownErrorProblem(Exception exception, HttpContext context)
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
