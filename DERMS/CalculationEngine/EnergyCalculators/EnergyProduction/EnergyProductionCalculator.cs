using CalculationEngine.Model.EnergyCalculations;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage;
using System.Linq;

namespace CalculationEngine.EnergyCalculators.EnergyProduction
{
    public class EnergyProductionCalculator : BaseTopologyCalculatingUnit<EnergyGenerator>
    {
        public EnergyProductionCalculator(IStorage<EnergyGenerator> storage) : base(PopulateNeededTypes(), storage)
        {
        }

        public override void Recalculate(EnergyBalanceCalculation energyBalance, long conductingEquipmentGid, float delta)
        {
            energyBalance.Production += delta;

            DMSType conductingEquipmentDMSType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(conductingEquipmentGid);
            energyBalance.DERProductions.First(x => x.DMSType == conductingEquipmentDMSType).TotalProduction += delta;
        }

        protected override void AdditionalProcessing(EnergyBalanceCalculation energyBalanceCalculation, EnergyGenerator entity)
        {
            base.AdditionalProcessing(energyBalanceCalculation, entity);
            var derProduction = energyBalanceCalculation.DERProductions.First(x => x.DMSType == entity.DMSType);
            derProduction.TotalProduction += ExtractValueFromEntity(entity);
        }

        private static List<DMSType> PopulateNeededTypes()
        {
            return new List<DMSType>() { DMSType.ENERGYSTORAGE, DMSType.SOLARGENERATOR, DMSType.WINDGENERATOR };
        }
    }
}
