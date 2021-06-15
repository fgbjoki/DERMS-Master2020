using CalculationEngine.Forecast.ProductionForecast.Formulas;
using CalculationEngine.Model.Forecast.ProductionForecast;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.DataTransferObjects;
using Common.Logger;
using Common.ServiceInterfaces.CalculationEngine;
using System.Collections.Generic;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.Production
{
    public class ProductionDataAggregator
    {
        private Dictionary<DMSType, IProductionFormula> productionFormulas;
        private IWeatherForecastStorage weatherForecastStorage;
        private IStorage<Generator> storage;

        public ProductionDataAggregator(IWeatherForecastStorage weatherForecastStorage, IStorage<Generator> storage)
        {
            this.storage = storage;
            this.weatherForecastStorage = weatherForecastStorage;

            productionFormulas = new Dictionary<DMSType, IProductionFormula>()
            {
                { DMSType.SOLARGENERATOR, new SolarPanelProductionFormula() },
                { DMSType.WINDGENERATOR, new WindGeneratorProductionFormula() }
            };
        }

        public List<ProductionForecast> GenerateData(int minutesInterval)
        {
            var weatherData = weatherForecastStorage.GetNextDayWeatherInfo();

            if (weatherData.Count == 0)
            {
                Logger.Instance.Log($"[{GetType().Name}] Weather data cannot be fetched for the next day.");
                return new List<ProductionForecast>(0);
            }

            FilterWeatherData(weatherData, minutesInterval);

            List<Generator> generators = storage.GetAllEntities();
            List<GeneratorProduction> generatorActivePowerForecast = new List<GeneratorProduction>();

            List<ProductionForecast> productionForecast = new List<ProductionForecast>(weatherData.Count);

            foreach (var weatherDataSample in weatherData)
            {
                ProductionForecast sampleForecast = new ProductionForecast();
                sampleForecast.WeahterData = weatherDataSample;

                productionForecast.Add(sampleForecast);

                foreach (var generator in generators)
                {
                    IProductionFormula formula;
                    if (!productionFormulas.TryGetValue(generator.DMSType, out formula))
                    {
                        Logger.Instance.Log($"[{GetType().Name}] Cannot find formula for entity with gid: 0x{generator.GlobalId:X16}");
                        continue;
                    }

                    float activePower = formula.CalculateProduction(generator, weatherDataSample);

                    GeneratorProduction forecast = new GeneratorProduction(generator.GlobalId, activePower);
                    sampleForecast.GeneratorProductions.Add(forecast);
                }
            }

            return productionForecast;
        }

        private void FilterWeatherData(List<WeatherDataInfo> weatherData, int interval)
        {
            int minutesADay = 60 * 24;
            for (int i = 0; i < minutesADay / interval; ++i)
            {
                weatherData.RemoveRange(i + 1, interval - 1);
            }
        }
    }
}
