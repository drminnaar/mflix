using System.Runtime.Serialization;

namespace MFlix.HttpApi.Models
{
    [DataContract(Namespace = "")]
    public sealed class TomatoesForSave
    {
        [DataMember]
        public string BoxOffice { get; init; } = string.Empty;

        [DataMember]
        public string Consensus { get; init; } = string.Empty;

        [DataMember]
        public double CriticRating { get; init; }

        [DataMember]
        public int CriticNumReviews { get; init; }

        [DataMember]
        public int CriticMeter { get; init; }

        [DataMember]
        public string Dvd { get; init; } = string.Empty;

        [DataMember]
        public int Fresh { get; init; }

        [DataMember]
        public string LastUpdated { get; init; } = string.Empty;

        [DataMember]
        public string Production { get; init; } = string.Empty;

        [DataMember]
        public int Rotten { get; init; }

        [DataMember]
        public double ViewerRating { get; init; }

        [DataMember]
        public int ViewerNumReviews { get; init; }

        [DataMember]
        public int ViewerMeter { get; init; }

        [DataMember]
        public string Website { get; init; } = string.Empty;
    }
}
