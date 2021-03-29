using System.Runtime.Serialization;

namespace MFlix.HttpApi.Models
{
    [DataContract(Namespace = "")]
    public sealed class TomatoesRating
    {
        [DataMember]
        public string? BoxOffice { get; init; } = string.Empty;

        [DataMember]
        public string? Consensus { get; init; } = string.Empty;

        [DataMember]
        public Critic? Critic { get; init; } = new Critic();

        [DataMember]
        public string? Dvd { get; init; }

        [DataMember]
        public int? Fresh { get; init; }

        [DataMember]
        public string? LastUpdated { get; init; }

        [DataMember]
        public string? Production { get; init; } = string.Empty;

        [DataMember]
        public int? Rotten { get; init; }

        [DataMember]
        public Viewer? Viewer { get; init; } = new Viewer();

        [DataMember]
        public string? Website { get; init; } = string.Empty;
    }
}
