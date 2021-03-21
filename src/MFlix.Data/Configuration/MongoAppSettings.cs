namespace MFlix.Data.Configuration
{
    public sealed class MongoAppSettings
    {
        internal const string ConfigurationSectionName = nameof(MongoAppSettings);

        public string ConnectionString { get; init; } = string.Empty;
    }
}
