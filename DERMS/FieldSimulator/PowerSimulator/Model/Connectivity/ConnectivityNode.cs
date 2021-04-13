using System.Collections.Generic;

namespace FieldSimulator.PowerSimulator.Model.Connectivity
{
    public class ConnectivityNode : IdentifiedObject
    {
        public ConnectivityNode(long globalId) : base(globalId)
        {
            Terminals = new List<Terminal>();
        }

        public List<Terminal> Terminals { get; set; }
    }
}
