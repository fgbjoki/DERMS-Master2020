using System.Collections.Generic;

namespace CalculationEngine.Model.Topology.Transaction
{
    public class ConductingEquipment : ConnectivityObject
    {
        public ConductingEquipment(long globalId) : base(globalId)
        {
            Terminals = new List<Terminal>();
        }

        public List<Terminal> Terminals { get; private set; }

        public void ConnectToTerminal(Terminal terminal)
        {
            Terminals.Add(terminal);
            terminal.ConductingEquipment = this;
        }
    }
}
