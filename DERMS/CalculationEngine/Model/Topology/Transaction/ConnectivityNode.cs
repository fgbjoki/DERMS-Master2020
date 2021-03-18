using System.Collections.Generic;

namespace CalculationEngine.Model.Topology.Transaction
{
    public class ConnectivityNode : ConnectivityObject
    {
        public ConnectivityNode(long globalId) : base(globalId)
        {
            Terminals = new List<Terminal>();
        }

        public List<Terminal> Terminals { get; private set; }

        public void AddTerminal(Terminal terminal)
        {
            Terminals.Add(terminal);
            terminal.ConnectivityNode = this;
        }
    }
}
