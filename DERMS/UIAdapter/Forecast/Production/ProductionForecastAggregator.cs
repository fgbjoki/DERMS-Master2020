using Common.Communication;
using Common.DataTransferObjects.CalculationEngine;
using Common.Logger;
using Common.ServiceInterfaces.CalculationEngine;
using Common.UIDataTransferObject.Forecast.Production;
using System;
using System.Collections.Generic;

namespace UIAdapter.Forecast.Production
{
    public class ProductionForecastAggregator : Common.ServiceInterfaces.UIAdapter.IProductionForecast
    {
        private WCFClient<IProductionForecast> productionForecastClient;

        public ProductionForecastAggregator()
        {
            productionForecastClient = new WCFClient<IProductionForecast>("ceProductionForecast");
        }

        public ProductionForecastDTO GetProductionForecast(int hours)
        {
            ProductionForecastDTO dto = new ProductionForecastDTO();
            ForecastDTO ceForecast;

            try
            {
                ceForecast = productionForecastClient.Proxy.ForecastProductionHourly(hours);
                if (ceForecast == null)
                {
                    return dto;
                }
            }
            catch (Exception e)
            {
                Logger.Instance.Log($"[{GetType().Name}] Couldn't fetch data from \'Calculation Engine\'. More info:\n{e.Message}\n");
                return dto;
            }

            dto = CreateGeneratorProductionForecast(ceForecast.ProductionForecastSamples);

            return dto;
        }

        private ProductionForecastDTO CreateGeneratorProductionForecast(Dictionary<DateTime, List<ForecastSampleDTO>> productionForecastSamples)
        {
            ProductionForecastDTO productionForecastDTO = new ProductionForecastDTO();
            TotalProductionForecast timedForecast = new TotalProductionForecast();

            Dictionary<long, List<GeneratorProductionForecast>> generatorsForecast = new Dictionary<long, List<GeneratorProductionForecast>>();

            foreach (var samples in productionForecastSamples)
            {
                Tuple<DateTime, float> totalSampleProduction;

                float totalActivePower = 0;
                foreach (var sample in samples.Value)
                {
                    List<GeneratorProductionForecast> generatorForecast;
                    if (!generatorsForecast.TryGetValue(sample.GeneratorGid, out generatorForecast))
                    {
                        generatorForecast = new List<GeneratorProductionForecast>();
                        generatorsForecast.Add(sample.GeneratorGid, generatorForecast);
                    }

                    generatorForecast.Add(new GeneratorProductionForecast() { ActivePower = sample.ActivePower, DateTime = samples.Key });

                    totalActivePower += sample.ActivePower;
                }

                totalSampleProduction = new Tuple<DateTime, float>(samples.Key, totalActivePower);
                timedForecast.TotalForecast.Add(totalSampleProduction);
            }

            productionForecastDTO.GeneratorProductionForecasts = generatorsForecast;
            productionForecastDTO.TimedForecast = timedForecast;

            return productionForecastDTO;
        }
    }
}
