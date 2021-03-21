using System;
using FluentValidation.Results;

namespace Grpc.Core
{
    public static class GrpcCoreExtensions
    {
        public static Metadata ToMetadata(this ValidationResult validationResult)
        {
            _ = validationResult ?? throw new ArgumentNullException(nameof(validationResult));

            if (validationResult.IsValid)
                return Metadata.Empty;

            var metadata = new Metadata();

            foreach (var error in validationResult.Errors)
            {
                error.FormattedMessagePlaceholderValues.TryGetValue("CollectionIndex", out var collectionIndex);
                collectionIndex = collectionIndex is null ? string.Empty : $"-{collectionIndex}";

                error.FormattedMessagePlaceholderValues.TryGetValue("PropertyName", out var propertyName);
                propertyName = propertyName is null ? string.Empty : propertyName.ToString();

                var entryKey = string.IsNullOrWhiteSpace($"{propertyName}{collectionIndex}")
                    ? Guid.NewGuid().ToString()
                    : $"{propertyName}{collectionIndex}";

                metadata.Add(new Metadata.Entry(entryKey, error.ErrorMessage));
            }

            return metadata;
        }
    }
}
