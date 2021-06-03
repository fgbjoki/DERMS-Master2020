using CalculationEngine.Model.Forecast.ProductionForecast;
using System.Collections.Generic;
using Common.GDA;
using Common.AbstractModel;

namespace CalculationEngine.TransactionProcessing.StorageItemCreators.Forecast
{
    public class WindGeneratorProductionForecastStorageItemCreator : ProductionForecastStorageItemCreator<WindGenerator>
    {
        protected override WindGenerator InsantiateEntity(ResourceDescription rd)
        {
            return new WindGenerator(rd.Id);
        }

        protected override void PopulateEntityProperties(WindGenerator entity, ResourceDescription rd)
        {
            base.PopulateEntityProperties(entity, rd);
            entity.CutOutSpeed = rd.GetProperty(ModelCode.WINDGENERATOR_CUTOUTSPEED).AsFloat();
            entity.StartUpSpeed = rd.GetProperty(ModelCode.WINDGENERATOR_STARTUPSPEED).AsFloat();
            entity.NominalSpeed = rd.GetProperty(ModelCode.WINDGENERATOR_NOMINALSPEED).AsFloat();
        }

        public override Dictionary<DMSType, List<ModelCode>> GetNeededProperties()
        {
            var properties = base.GetNeededProperties();
            properties[DMSType.WINDGENERATOR].Add(ModelCode.WINDGENERATOR_CUTOUTSPEED);
            properties[DMSType.WINDGENERATOR].Add(ModelCode.WINDGENERATOR_STARTUPSPEED);
            properties[DMSType.WINDGENERATOR].Add(ModelCode.WINDGENERATOR_NOMINALSPEED);

            return properties;
        }
    }
}
