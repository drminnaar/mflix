using System;
using FluentValidation.Results;

namespace Grpc.Core
{
    public static class GrpcCoreExtensions
    {
        public static Metadata ToMetadata(this ValidationResult validationResult)
        {
            _ = validationResult ?? throw new ArgumentNullException(nameof(validationResult));
            return validationResult.IsValid ? Metadata.Empty : DetermineMetadata(validationResult);
        }

        private static Metadata DetermineMetadata(ValidationResult validationResult)
        {
            var metadata = new Metadata();

            foreach (var error in validationResult.Errors)
            {
                var entryKey = DetermineKey(error);
                metadata.Add(new Metadata.Entry(entryKey, error.ErrorMessage));
            }

            return metadata;
        }

        private static string DetermineKey(ValidationFailure error)
        {
            error.FormattedMessagePlaceholderValues.TryGetValue("CollectionIndex", out var collectionIndex);
            collectionIndex = collectionIndex is null
                ? string.Empty
                : $"-{collectionIndex}";

            error.FormattedMessagePlaceholderValues.TryGetValue("PropertyName", out var propertyName);
            propertyName = propertyName is null
                ? string.Empty
                : propertyName.ToString()?.Trim().Replace(" ", string.Empty, StringComparison.InvariantCulture);

            var entryKey = string.IsNullOrWhiteSpace($"{propertyName}{collectionIndex}")
                ? Guid.NewGuid().ToString()
                : $"{propertyName}{collectionIndex}";

            return entryKey;
        }
    }
}
