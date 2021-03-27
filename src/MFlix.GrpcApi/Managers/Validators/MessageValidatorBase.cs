using FluentValidation;
using Google.Protobuf;
using Grpc.Core;

namespace MFlix.GrpcApi.Managers.Validators
{
    public abstract class MessageValidatorBase<T> : AbstractValidator<T> where T : IMessage
    {
        protected MessageValidatorBase() : base()
        {
        }

        public bool IsValid(T entity, out Metadata metadata)
        {
            var validationResult = Validate(entity);
            metadata = validationResult.ToMetadata();
            return validationResult.IsValid;
        }
    }
}
