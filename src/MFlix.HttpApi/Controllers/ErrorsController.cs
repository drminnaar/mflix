using System;
using System.Collections.Generic;
using System.Linq;
using Grpc.Core;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RpcStatusCode = Grpc.Core.StatusCode;

namespace MFlix.HttpApi.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public sealed class ErrorsController : ApiControllerBase
    {
        internal const string ErrorsPath = "errors";
        private readonly ILogger<ErrorsController> _logger;
        private readonly IWebHostEnvironment _environment;

        public ErrorsController(ILogger<ErrorsController> logger, IWebHostEnvironment environment)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        [Route(ErrorsPath)]
        public IActionResult Error()
        {
            var error = HttpContext
                .Features
                .Get<IExceptionHandlerFeature>()
                ?.Error;

            if (error is null)
                return NoContent();

            return error switch
            {
                RpcException ex when (ex.StatusCode == RpcStatusCode.InvalidArgument) => HandleInvalidArgument(ex),
                RpcException ex when (ex.StatusCode == RpcStatusCode.NotFound) => HandleNotFound(ex),
                _ => HandleUnexpectedException(error),
            };
        }

        private IActionResult HandleInvalidArgument(RpcException exception)
        {
            _logger.LogWarning(exception.Message);
            HttpContext.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
            return new ObjectResult(CreateValidationProblem(exception));
        }

        private ValidationProblemDetails CreateValidationProblem(RpcException exception)
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
                Instance = HttpContext.Request?.Path.Value ?? string.Empty,
                Status = StatusCodes.Status422UnprocessableEntity,
                Title = "There were validation errors",
                Type = $"https://httpstatuses.com/{StatusCodes.Status422UnprocessableEntity}"
            };
            problem.Extensions.Add("traceId", HttpContext.TraceIdentifier);
            return problem;
        }

        private IActionResult HandleNotFound(RpcException exception)
        {
            _logger.LogWarning(exception.Message);
            HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            return new ObjectResult(CreateNotFoundProblem());
        }

        private ProblemDetails CreateNotFoundProblem()
        {
            var problemDetails = new ProblemDetails()
            {
                Detail = string.Empty,
                Instance = HttpContext.Request?.Path.Value ?? string.Empty,
                Status = StatusCodes.Status404NotFound,
                Title = "Resource not found",
                Type = $"https://httpstatuses.com/{StatusCodes.Status404NotFound}"
            };
            problemDetails.Extensions.Add("traceId", HttpContext.TraceIdentifier);
            return problemDetails;
        }

        private IActionResult HandleUnexpectedException(Exception? exception)
        {
            if (exception is null)
            {
                _logger.LogError(StatusCodes.Status500InternalServerError, "An unknown server error occurred");
                HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                return new ObjectResult(CreateUnknownErrorProblem());
            }
            else
            {
                _logger.LogError(StatusCodes.Status500InternalServerError, exception, exception.Message);
                HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                return new ObjectResult(CreateKnownErrorProblem(exception));
            }
        }

        private ProblemDetails CreateUnknownErrorProblem()
        {
            var problemDetails = new ProblemDetails()
            {
                Detail = string.Empty,
                Instance = HttpContext.Request?.Path.Value ?? string.Empty,
                Status = StatusCodes.Status500InternalServerError,
                Title = "A server fault occurred",
                Type = $"https://httpstatuses.com/{StatusCodes.Status500InternalServerError}"
            };
            problemDetails.Extensions.Add("traceId", HttpContext.TraceIdentifier);
            return problemDetails;
        }

        private ProblemDetails CreateKnownErrorProblem(Exception exception)
        {
            var problemDetails = new ProblemDetails()
            {
                Instance = HttpContext.Request?.Path.Value ?? string.Empty,
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

            problemDetails.Extensions.Add("traceId", HttpContext.TraceIdentifier);

            return problemDetails;
        }
    }
}
