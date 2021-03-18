using Common.ComponentStorage;

namespace CalculationEngine.Model.Topology.Transaction
{
    public class Terminal : ConnectivityObject
    {
        public Terminal(long globalId) : base(globalId)
        {
        }

        public ConnectivityNode ConnectivityNode { get; set; }

        public ConductingEquipment ConductingEquipment { get; set; }

    }
}
