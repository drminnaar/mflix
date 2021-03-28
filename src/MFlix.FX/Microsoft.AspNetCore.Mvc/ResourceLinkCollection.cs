﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.AspNetCore.Mvc
{
    [CollectionDataContract(Name = "Links", Namespace = "")]
    public sealed class ResourceLinkCollection : IEnumerable<ResourceLink>
    {
        private readonly List<ResourceLink> _links = new();

        public ResourceLinkCollection()
        {
        }

        public ResourceLinkCollection(IEnumerable<ResourceLink> collection)
        {
            _links = new List<ResourceLink>(collection);
        }

        public void Add(ResourceLink item)
        {
            _links.Add(item);
        }

        public ResourceLinkCollection AddRange(IEnumerable<ResourceLink> items)
        {
            _links.AddRange(items);
            return this;
        }

        public IEnumerator<ResourceLink> GetEnumerator() => _links.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _links.GetEnumerator();
    }
}
