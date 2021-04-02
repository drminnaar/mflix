namespace MFlix.GqlApi.Infrastructure.Configuration
{
    public sealed record GrpcClientSettings
    {
        internal const string ConfigurationSectionName = nameof(GrpcClientSettings);

        public string Address { get; init; } = string.Empty;
    }
}
