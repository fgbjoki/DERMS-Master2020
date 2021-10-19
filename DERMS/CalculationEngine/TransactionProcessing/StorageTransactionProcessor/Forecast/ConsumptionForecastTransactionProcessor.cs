using CalculationEngine.Model.Forecast.ConsumptionForecast;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.ComponentStorage.StorageItemCreator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationEngine.TransactionProcessing.StorageTransactionProcessor.Forecast
{
    public class ConsumptionForecastTransactionProcessor : StorageTransactionProcessor<Consumer>
    {
        public ConsumptionForecastTransactionProcessor(IStorage<Consumer> storage, Dictionary<DMSType, IStorageItemCreator> storageItemCreators) : base(storage, storageItemCreators)
        {

        }

        protected override List<DMSType> GetPrimaryTypes()
        {
            return new List<DMSType>(1) { DMSType.ENERGYCONSUMER };
        }
    }
}
