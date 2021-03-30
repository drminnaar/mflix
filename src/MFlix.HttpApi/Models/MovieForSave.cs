using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace MFlix.HttpApi.Models
{
    [DataContract(Namespace = "")]
    public sealed class MovieForSave
    {
        [DataMember]
        public string Id { get; init; } = string.Empty;

        [DataMember]
        public string Title { get; init; } = string.Empty;

        [DataMember]
        public string Plot { get; init; } = string.Empty;

        [DataMember]
        public int Runtime { get; init; }

        [DataMember]
        public string Rated { get; init; } = string.Empty;

        [DataMember]
        public int Year { get; init; }

        [DataMember]
        public string Poster { get; init; } = string.Empty;

        [DataMember]
        public string Released { get; init; } = string.Empty;

        [DataMember]
        public IReadOnlyCollection<string> Genres { get; init; } = Enumerable.Empty<string>().ToList();

        [DataMember]
        public IReadOnlyCollection<string> Cast { get; init; } = Enumerable.Empty<string>().ToList();

        [DataMember]
        public IReadOnlyCollection<string> Directors { get; init; } = Enumerable.Empty<string>().ToList();
    }
}
