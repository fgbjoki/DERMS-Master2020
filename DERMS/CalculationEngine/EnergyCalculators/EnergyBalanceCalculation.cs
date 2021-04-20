namespace CalculationEngine.EnergyCalculators
{
    public class EnergyBalanceCalculation
    {
        public EnergyBalanceCalculation(long energySourceGid)
        {
            EnergySourceGid = energySourceGid;
        }

        public long EnergySourceGid { get; private set; }

        /// <summary>
        /// Consumption.
        /// </summary>
        public float Demand { get; set; }

        /// <summary>
        /// DER production.
        /// </summary>
        public float Production { get; set; }

        /// <summary>
        /// Imported energy from energy source.
        /// </summary>
        public float Imported { get; set; }
    }
}
