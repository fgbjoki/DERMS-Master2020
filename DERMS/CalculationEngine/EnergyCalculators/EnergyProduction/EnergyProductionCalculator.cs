using CalculationEngine.Model.EnergyCalculations;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage;
using System;

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

        public override void Recalculate(EnergyBalanceCalculation energyBalance, float delta)
        {
            energyBalance.Production += delta;
        }
    }
}
