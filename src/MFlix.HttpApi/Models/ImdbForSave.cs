using System.Runtime.Serialization;

namespace MFlix.HttpApi.Models
{
    [DataContract(Namespace = "")]
    public class ImdbForSave
    {
        [DataMember]
        public int Id { get; init; }

        [DataMember]
        public double? Rating { get; init; }

        [DataMember]
        public int? Votes { get; init; }
    }
}
