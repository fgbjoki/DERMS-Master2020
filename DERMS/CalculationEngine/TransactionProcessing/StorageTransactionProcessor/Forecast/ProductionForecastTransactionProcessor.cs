using CalculationEngine.Model.Forecast.ProductionForecast;
using Common.ComponentStorage;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage.StorageItemCreator;

namespace CalculationEngine.TransactionProcessing.StorageTransactionProcessor.Forecast
{
    public class ProductionForecastTransactionProcessor : StorageTransactionProcessor<Generator>
    {
        public ProductionForecastTransactionProcessor(IStorage<Generator> storage, Dictionary<DMSType, IStorageItemCreator> storageItemCreators) : base(storage, storageItemCreators)
        {
        }

        protected override List<DMSType> GetPrimaryTypes()
        {
            return new List<DMSType>(2) { DMSType.WINDGENERATOR, DMSType.SOLARGENERATOR };
        }
    }
}
