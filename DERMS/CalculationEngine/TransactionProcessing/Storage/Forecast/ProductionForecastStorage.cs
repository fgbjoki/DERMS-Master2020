using CalculationEngine.Model.Forecast.ProductionForecast;
using CalculationEngine.TransactionProcessing.StorageItemCreators.Forecast;
using CalculationEngine.TransactionProcessing.StorageTransactionProcessor.Forecast;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.ComponentStorage.StorageItemCreator;
using System.Collections.Generic;

namespace CalculationEngine.TransactionProcessing.Storage.Forecast
{
    public class ProductionForecastStorage : Storage<Generator>
    {
        public ProductionForecastStorage() : base("Production forecast storage")
        {
        }

        public override List<IStorageTransactionProcessor> GetStorageTransactionProcessors()
        {
            Dictionary<DMSType, IStorageItemCreator> creators = new Dictionary<DMSType, IStorageItemCreator>()
            {
                { DMSType.WINDGENERATOR, new WindGeneratorProductionForecastStorageItemCreator() },
                { DMSType.SOLARGENERATOR, new SolarPanelProductionForecastStorageItemCreator() }
            };

            return new List<IStorageTransactionProcessor>(1) { new ProductionForecastTransactionProcessor(this, creators) };
        }

        protected override IStorage<Generator> CreateNewStorage()
        {
            return new ProductionForecastStorage();
        }
    }
}
