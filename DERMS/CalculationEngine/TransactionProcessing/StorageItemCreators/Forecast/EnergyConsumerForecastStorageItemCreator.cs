using CalculationEngine.Model.Forecast.ConsumptionForecast;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.ComponentStorage.StorageItemCreator;
using Common.GDA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationEngine.TransactionProcessing.StorageItemCreators.Forecast
{
    public class EnergyConsumerForecastStorageItemCreator : StorageItemCreator
    {
        public override IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            Consumer entity = InsantiateEntity(rd);

            PopulateEntityProperties(entity, rd);

            return entity;
        }

        protected void PopulateEntityProperties(Consumer entity, ResourceDescription rd)
        {
            entity.Pfixed = rd.GetProperty(ModelCode.ENERGYCONSUMER_PFIXED).AsFloat();
            entity.Type = (ConsumerType)rd.GetProperty(ModelCode.ENERGYCONSUMER_TYPE).AsEnum();
        }

        protected Consumer InsantiateEntity(ResourceDescription rd)
        {
            return new Consumer(rd.Id);
        }


        public override Dictionary<DMSType, List<ModelCode>> GetNeededProperties()
        {
            return new Dictionary<DMSType, List<ModelCode>>()
            {
                { DMSType.ENERGYCONSUMER, new List<ModelCode>() { ModelCode.ENERGYCONSUMER_PFIXED, ModelCode.ENERGYCONSUMER_TYPE } }
            };
        }
    }
}
