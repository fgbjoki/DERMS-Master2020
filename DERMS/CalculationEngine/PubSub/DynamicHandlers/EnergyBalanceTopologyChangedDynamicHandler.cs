using CalculationEngine.EnergyCalculators;
using Common.PubSub;

namespace CalculationEngine.PubSub.DynamicHandlers
{
    public class EnergyBalanceTopologyChangedDynamicHandler : IDynamicHandler
    {
        private IEnergyBalanceCalculator energyBalanceCalculator;

        public EnergyBalanceTopologyChangedDynamicHandler(IEnergyBalanceCalculator energyBalanceCalculator)
        {
            this.energyBalanceCalculator = energyBalanceCalculator;
        }

        public void ProcessChanges(object changes)
        {
            energyBalanceCalculator.PerformCalculation();
        }
    }
}
