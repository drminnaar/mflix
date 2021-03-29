using System;
using System.Runtime.Serialization;

namespace MFlix.HttpApi.Models
{
    [DataContract(Namespace = "")]
    public sealed class Cast
    {
        public Cast()
        {
        }

        public Cast(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        [DataMember]
        public string Name { get; set; } = string.Empty;
    }
}
