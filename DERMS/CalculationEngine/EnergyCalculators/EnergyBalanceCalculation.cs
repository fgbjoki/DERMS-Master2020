namespace CalculationEngine.EnergyCalculators
{
    public class EnergyBalanceCalculation
    {
        public EnergyBalanceCalculation(long energySourceGid)
        {
            EnergySourceGid = energySourceGid;
        }

        public long EnergySourceGid { get; private set; }

        public float Demand { get; set; }

        public float Production { get; set; }

        public float Imported { get; set; }
    }
}
