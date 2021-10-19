using CalculationEngine.Model.Forecast.ConsumptionForecast;
using CalculationEngine.TransactionProcessing.StorageItemCreators.Forecast;
using CalculationEngine.TransactionProcessing.StorageTransactionProcessor.Forecast;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.ComponentStorage.StorageItemCreator;
using System;
using System.Collections.Generic;

namespace CalculationEngine.TransactionProcessing.Storage.Forecast
{
    public class ConsumptionForecastStorage : Storage<Consumer>
    {
        public ConsumptionForecastStorage() : base("Consumption forecast storage")
        {

        }

        public override List<IStorageTransactionProcessor> GetStorageTransactionProcessors()
        {
            Dictionary<DMSType, IStorageItemCreator> creators = new Dictionary<DMSType, IStorageItemCreator>()
            {
                { DMSType.ENERGYCONSUMER, new EnergyConsumerForecastStorageItemCreator() }
            };

            return new List<IStorageTransactionProcessor>(1) { new ConsumptionForecastTransactionProcessor(this, creators) };
        }

        protected override IStorage<Consumer> CreateNewStorage()
        {
            return new ConsumptionForecastStorage();
        }
    }
}
