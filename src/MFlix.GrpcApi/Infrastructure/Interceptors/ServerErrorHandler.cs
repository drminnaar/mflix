using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using MFlix.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MFlix.GrpcApi.Infrastructure.Interceptors
{
    public sealed class ServerErrorHandler : Interceptor
    {
        private readonly ILogger<ServerErrorHandler> _logger;
        private readonly IWebHostEnvironment _environment;

        public ServerErrorHandler(ILogger<ServerErrorHandler> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));
            if (context is null) throw new ArgumentNullException(nameof(context));
            if (continuation is null) throw new ArgumentNullException(nameof(continuation));

            return UnaryServerHandler();

            async Task<TResponse> UnaryServerHandler()
            {
                try
                {
                    return await continuation(request, context).ConfigureAwait(true);
                }
                catch (EntityNotFoundException entityNotFoundException)
                {
                    _logger.LogWarning(
                        entityNotFoundException,
                        "{ExceptionMessage}",
                        entityNotFoundException.Message);

                    throw new RpcException(
                        new Status(StatusCode.NotFound, "Entity not found"),
                        new Metadata
                        {
                            { nameof(entityNotFoundException.EntityName), entityNotFoundException.EntityName },
                            { nameof(entityNotFoundException.EntityId), entityNotFoundException.EntityId }
                        },
                        $"An entity having id '{entityNotFoundException.EntityId}' could not be found");
                }
                catch (Exception exception)
                {
                    _logger.LogError(
                        exception,
                        "{ExceptionMessage}",
                        exception.Message);

                    throw _environment.IsDevelopment()
                        ? exception
                        : new RpcException(new Status(StatusCode.Internal, "There was an unexpected server fault"));
                }
            }
        }
    }
}
