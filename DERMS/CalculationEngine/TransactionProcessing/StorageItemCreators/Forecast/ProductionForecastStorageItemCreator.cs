using Common.ComponentStorage.StorageItemCreator;
using System;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.GDA;
using CalculationEngine.Model.Forecast.ProductionForecast;

namespace CalculationEngine.TransactionProcessing.StorageItemCreators.Forecast
{
    public abstract class ProductionForecastStorageItemCreator<T> : StorageItemCreator
        where T : Generator
    {
        public override IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            T entity = InsantiateEntity(rd);

            PopulateEntityProperties(entity, rd);

            return entity;
        }

        protected virtual void PopulateEntityProperties(T entity, ResourceDescription rd)
        {
            entity.NominalPower = rd.GetProperty(ModelCode.DER_NOMINALPOWER).AsFloat();
        }

        protected abstract T InsantiateEntity(ResourceDescription rd);

        public override Dictionary<DMSType, List<ModelCode>> GetNeededProperties()
        {
            return new Dictionary<DMSType, List<ModelCode>>()
            {
                { DMSType.SOLARGENERATOR, new List<ModelCode>() { ModelCode.DER_NOMINALPOWER } },
                { DMSType.WINDGENERATOR, new List<ModelCode>() { ModelCode.DER_NOMINALPOWER } }
            };
        }
    }
}
