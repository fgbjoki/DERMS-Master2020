using FieldSimulator.PowerSimulator.Model.Equipment;

namespace FieldSimulator.PowerSimulator.Model.Connectivity
{
    public class Terminal : IdentifiedObject
    {
        public Terminal(long globalId) : base(globalId)
        {
        }

        public ConductingEquipment ConductingEquipment { get; set; }
        public string ConductingEquipmentID { get; set; }

        public ConnectivityNode ConnectivityNode { get; set; }
        public string ConnectivityNodeID { get; set; }

        public override void Update(DERMS.IdentifiedObject cimObject)
        {
            base.Update(cimObject);

            DERMS.Terminal terminalCim = cimObject as DERMS.Terminal;

            if (terminalCim == null)
            {
                return;
            }

            ConductingEquipmentID = terminalCim.ConductingEquipment.ID;
            ConnectivityNodeID = terminalCim.ConnectivityNode.ID;
        }
    }
}
