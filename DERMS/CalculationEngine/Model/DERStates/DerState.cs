using Common.ComponentStorage;

namespace CalculationEngine.Model.DERStates
{
    public class DERState : IdentifiedObject
    {
        private float activePower;

        public DERState(long globalId) : base(globalId)
        {
        }

        public float ActivePower
        {
            get
            {
                return !IsEnergized ? 0 : activePower;
            }
            set
            {
                activePower = value;
            }
        }

        public long ActivePowerMeasurementGid { get; set; }

        public bool IsEnergized { get; set; }
    }
}
