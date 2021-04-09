using CalculationEngine.Model.EnergyCalculations;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage;

namespace CalculationEngine.EnergyCalculators.EnergyProduction
{
    public class EnergyProductionCalculator : BaseTopologyCalculatingUnit<EnergyGenerator>
    {
        public EnergyProductionCalculator(IStorage<EnergyGenerator> storage) : base(PopulateNeededTypes(), storage)
        {
        }

        private static List<DMSType> PopulateNeededTypes()
        {
            return new List<DMSType>() { DMSType.ENERGYSTORAGE, DMSType.SOLARGENERATOR, DMSType.WINDGENERATOR };
        }
    }
}
