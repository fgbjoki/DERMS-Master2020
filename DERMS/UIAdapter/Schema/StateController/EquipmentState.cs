namespace UIAdapter.Schema.StateController
{ 
    public class EquipmentState
    {
        public EquipmentState(long globalId)
        {
            GlobalId = globalId;
        }

        public long GlobalId { get; private set; }
        public bool IsEnergized { get; set; }
        public bool DoesConduct { get; set; }
    }
}
