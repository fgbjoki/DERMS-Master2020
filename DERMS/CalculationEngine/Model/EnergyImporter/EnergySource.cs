using Common.ComponentStorage;

namespace CalculationEngine.Model.EnergyImporter
{
    public class EnergySource : IdentifiedObject
    {
        public EnergySource(long globalId) : base(globalId)
        {
        }

        public float ActivePower { get; set; }
        public long ActivePowerMeasurementGid { get; set; }
    }
}
