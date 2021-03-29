using System.Runtime.Serialization;

namespace MFlix.HttpApi.Models
{
    [DataContract(Namespace = "")]
    public sealed class Viewer
    {
        [DataMember]
        public double? Rating { get; init; }

        [DataMember]
        public int? NumReviews { get; init; }

        [DataMember]
        public int? Meter { get; init; }
    }
}
