using CalculationEngine.Model.EnergyCalculations;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage;

namespace CalculationEngine.EnergyCalculators.EnergyConsumption
{
    public class EnergyConsumptionCalculator : BaseTopologyCalculatingUnit<EnergyConsumer>
    {
        public EnergyConsumptionCalculator(IStorage<EnergyConsumer> storage) : base(PopulateNeededTypes(), storage)
        {
        }

        private static List<DMSType> PopulateNeededTypes()
        {
            return new List<DMSType>() { DMSType.ENERGYCONSUMER };
        }
    }
}
