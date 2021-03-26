using System;
using System.Runtime.Serialization;

namespace MFlix.Data
{
    [Serializable]
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException() { }

        public EntityNotFoundException(string message) : base(message) { }

        public EntityNotFoundException(string message, Exception inner) : base(message, inner) { }

        protected EntityNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }

        public string EntityId { get; init; } = string.Empty;
        public string EntityName { get; init; } = string.Empty;
        public string EntityType { get; init; } = string.Empty;
    }
}
