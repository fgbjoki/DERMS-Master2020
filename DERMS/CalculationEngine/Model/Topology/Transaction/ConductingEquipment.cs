using Common.AbstractModel;
using Common.ComponentStorage;
using System.Collections.Generic;

namespace CalculationEngine.Model.Topology.Transaction
{
    public class ConductingEquipment : ConnectivityObject
    {
        public ConductingEquipment(long globalId) : base(globalId)
        {
            DMSType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(globalId);
            Terminals = new List<Terminal>();
        }

        public DMSType DMSType { get; private set; }

        public List<Terminal> Terminals { get; private set; }

        public void ConnectToTerminal(Terminal terminal)
        {
            Terminals.Add(terminal);
            terminal.ConductingEquipment = this;
        }
    }
}
