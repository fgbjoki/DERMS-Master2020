using NServiceBus;
using System.Collections.Generic;

namespace Common.PubSub.Messages
{
    public class SchemaGraphChanged : IEvent
    {
        public SchemaGraphChanged()
        {

        }

        public SchemaGraphChanged(Dictionary<long, List<long>> parentChildBranches, long interConnectedBreakerGid)
        {
            ParentChildBranches = parentChildBranches;
            InterConnectedBreakerGid = interConnectedBreakerGid;

        }

        public long InterConnectedBreakerGid { get; set; }

        public Dictionary<long, List<long>> ParentChildBranches { get; set; }
    }
}
