using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Grpc.Core;
using HotChocolate;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RpcStatusCode = Grpc.Core.StatusCode;

namespace MFlix.GqlApi.Infrastructure.Filters
{
    public sealed class ServerErrorFilter : IErrorFilter
    {
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _environment;

        public ServerErrorFilter(ILogger logger, IWebHostEnvironment environment)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        public IError OnError(IError error)
        {
            if (error is null)
                throw new ArgumentNullException(nameof(error));

            var result = error.Exception switch
            {
                RpcException ex when (ex.StatusCode == RpcStatusCode.InvalidArgument) => HandleInvalidArgument(error, ex),
                RpcException ex when (ex.StatusCode == RpcStatusCode.NotFound) => HandleNotFound(error, ex),
                _ => HandleUnexprectedException(error)
            };

            return result;
        }

        private IError HandleNotFound(IError error, RpcException exception)
        {
            _logger.LogWarning(404, exception, "Resource not found");

            if (_environment.IsDevelopment())
                return error;

            return ErrorBuilder
                .New()
                .SetCode(ErrorCode.ResourceNotFound)
                .SetMessage("Resource not found")
                .SetPath(error.Path)
                .Build();
        }

        private IError HandleInvalidArgument(IError error, RpcException exception)
        {
            _logger.LogWarning(422, exception, "Input validation failures");

            if (_environment.IsDevelopment())
                return error;

            var failures = exception
                .Trailers
                .ToValidationResult()
                .Errors
                .Select(failure =>
                {
                    var propertyName = failure
                        .PropertyName
                        .ToUpper(CultureInfo.InvariantCulture)
                        .Replace(" ", "", StringComparison.InvariantCultureIgnoreCase);

                    return new KeyValuePair<string, string>(
                        key: $"{ErrorCode.InvalidUserInput}_{propertyName}",
                        value: failure.ErrorMessage);
                })
                .ToList();

            var validationError = ErrorBuilder
                .New()
                .SetCode(ErrorCode.InvalidUserInput)
                .SetMessage("Input validation failures")
                .SetPath(error.Path)
                .Build();

            foreach (var failure in failures)
                validationError = validationError.SetExtension(failure.Key, failure.Value);

            return validationError;
        }

        private IError HandleUnexprectedException(IError error)
        {
            _logger.LogError(500, error.Exception, $"Server fault.");

            if (_environment.IsDevelopment())
                return error;

            return ErrorBuilder
                .New()
                .SetCode(ErrorCode.ServerFault)
                .SetMessage("Server fault")
                .SetPath(error.Path)
                .Build();
        }
    }
}
