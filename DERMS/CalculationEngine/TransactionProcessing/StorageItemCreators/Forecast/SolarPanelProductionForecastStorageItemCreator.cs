using CalculationEngine.Model.Forecast.ProductionForecast;
using Common.AbstractModel;
using Common.GDA;
using System.Collections.Generic;

namespace CalculationEngine.TransactionProcessing.StorageItemCreators.Forecast
{
    public class SolarPanelProductionForecastStorageItemCreator : ProductionForecastStorageItemCreator<SolarPanel>
    {
        protected override SolarPanel InsantiateEntity(ResourceDescription rd)
        {
            return new SolarPanel(rd.Id);
        }
    }
}
