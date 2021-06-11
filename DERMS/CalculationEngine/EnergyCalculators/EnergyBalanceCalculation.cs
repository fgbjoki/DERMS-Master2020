using Common.AbstractModel;
using System.Collections.Generic;

namespace CalculationEngine.EnergyCalculators
{
    public class DERProduction
    {
        public DERProduction(DMSType dmsType)
        {
            DMSType = dmsType;
        }

        public DMSType DMSType { get; private set; }
        public float TotalProduction { get; set; }
    }

    public class EnergyBalanceCalculation
    {
        public EnergyBalanceCalculation(long energySourceGid)
        {
            EnergySourceGid = energySourceGid;
            DERProductions = new List<DERProduction>()
            {
                new DERProduction(DMSType.SOLARGENERATOR),
                new DERProduction(DMSType.WINDGENERATOR),
                new DERProduction(DMSType.ENERGYSTORAGE)
            };
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

        public List<DERProduction> DERProductions { get; set; }
    }
}
