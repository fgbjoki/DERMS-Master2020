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

        public override void Recalculate(EnergyBalanceCalculation energyBalance, long conductingEquipmentGid, float delta)
        {
            DMSType conductingEquipmentDMSType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(conductingEquipmentGid);
            energyBalance.Demand += delta;
        }

        protected override void AdditionalProcessing(EnergyBalanceCalculation energyBalanceCalculation, EnergyConsumer entity)
        {
            base.AdditionalProcessing(energyBalanceCalculation, entity);
            var demand = energyBalanceCalculation.Demand;
            demand += ExtractValueFromEntity(entity);
        }
    }
}
