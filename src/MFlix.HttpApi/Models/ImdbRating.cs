using System.Runtime.Serialization;

namespace MFlix.HttpApi.Models
{
    [DataContract(Namespace = "")]
    public sealed class ImdbRating
    {
        [DataMember]
        public int Id { get; init; }

        [DataMember]
        public double? Rating { get; init; }

        [DataMember]
        public int? Votes { get; init; }
    }
}
