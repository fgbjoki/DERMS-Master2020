using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.Models.Schema
{
    public class SchemaGraphWrapper
    {
        public SchemaGraphWrapper(SchemaNode root, Dictionary<long, SchemaNode> nodes, long interConnectedBreakerGid)
        {
            Root = root;
            Nodes = nodes;
            InterConnectedBreakerGlobalId = interConnectedBreakerGid;
        }

        public SchemaNode Root { get; private set; }

        public Dictionary<long, SchemaNode> Nodes { get; private set; }

        public long InterConnectedBreakerGlobalId { get; private set; }
    }
}
