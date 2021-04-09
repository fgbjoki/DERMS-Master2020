using CalculationEngine.EnergyCalculators.EnergyConsumption;
using CalculationEngine.EnergyCalculators.EnergyProduction;
using CalculationEngine.Model.EnergyCalculations;
using CalculationEngine.TopologyAnalysis;
using Common.ComponentStorage;
using Common.PubSub;
using System;
using System.Collections.Generic;
using Common.PubSub.Subscriptions;

namespace CalculationEngine.EnergyCalculators
{
    public class EnergyBalanceCalculator : ISubscriber
    {
        private ITopologyCalculatingUnit energyProduction;
        private ITopologyCalculatingUnit energyConsumption;

        private ITopologyReader topologyReader;

        public EnergyBalanceCalculator(IStorage<EnergyGenerator> generatorStorage, IStorage<EnergyConsumer> consumerStorage, ITopologyAnalysis topologyAnalysisController)
        {
            energyProduction = new EnergyProductionCalculator(generatorStorage);
            energyConsumption = new EnergyConsumptionCalculator(consumerStorage);

            topologyReader = topologyAnalysisController.CreateReader();
        }

        public IEnumerable<ISubscription> GetSubscriptions()
        {
            // TODO dynamic discrete (breaker state) and analog (der active power)
            throw new NotImplementedException();
        }
    }
}
