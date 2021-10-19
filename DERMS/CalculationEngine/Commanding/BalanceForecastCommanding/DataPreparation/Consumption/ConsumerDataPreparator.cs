using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.InitialPopulationCreation.Model;
using CalculationEngine.Model.DERCommanding;
using CalculationEngine.Model.Forecast.ConsumptionForecast;
using Common.ComponentStorage;
using System.Collections.Generic;
using System.Linq;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.DataPreparation.Consumption
{
    public class ConsumerDataPreparator
    {
        private IStorage<DistributedEnergyResource> ders;

        public ConsumerDataPreparator(IStorage<DistributedEnergyResource> ders)
        {
            this.ders = ders;
        }

        public List<EnergyConsumerEntity> CreateEntities()
        {
            List<EnergyConsumerEntity> consumers = new List<EnergyConsumerEntity>();
            foreach (var consumer in ders.GetAllEntities().Where(x => x.DMSType == Common.AbstractModel.DMSType.ENERGYCONSUMER).Cast<Consumer>())
            {
                var consumerEntity = new EnergyConsumerEntity()
                {
                    GlobalId = consumer.GlobalId,
                    Type = consumer.Type,
                    Pfixed = consumer.Pfixed
                };

                consumers.Add(consumerEntity);
            }

            return consumers;
        }
    }
}
