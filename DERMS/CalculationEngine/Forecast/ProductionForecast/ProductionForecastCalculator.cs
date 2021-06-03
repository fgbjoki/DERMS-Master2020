using CalculationEngine.Forecast.ProductionForecast.Formulas;
using CalculationEngine.Forecast.WeatherForecast;
using CalculationEngine.Model.Forecast.ProductionForecast;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.DataTransferObjects.CalculationEngine;
using Common.Logger;
using Common.ServiceInterfaces.CalculationEngine;
using System;
using System.Collections.Generic;

namespace CalculationEngine.Forecast.ProductionForecast
{
    public class ProductionForecastCalculator : IProductionForecast
    {
        private IStorage<Generator> storage;
        private Dictionary<DMSType, IProductionFormula> productionFormulas;
        private IWeatherForecastStorage weatherForecast;


        public ProductionForecastCalculator(IStorage<Generator> storage, IWeatherForecastStorage weatherForecast)
        {
            this.weatherForecast = weatherForecast;
            this.storage = storage;

            InitializeProductionFormulas();
        }

        public ForecastDTO ForecastProductionMinutely(int minutes)
        {
            List<WeatherDataInfo> dataInfos = weatherForecast.GetMinutesWeatherInfo(minutes);
            return CalculateProduction(dataInfos);
        }

        public ForecastDTO ForecastProductionHourly(int hours)
        {
            List<WeatherDataInfo> dataInfos = weatherForecast.GetHourlyWeatherInfo(hours);
            return CalculateProduction(dataInfos);
        }

        private ForecastDTO CalculateProduction(List<WeatherDataInfo> dataInfos)
        {
            List<Generator> generators = storage.GetAllEntities();

            ForecastDTO dto = new ForecastDTO();
            dto.ProductionForecastSamples = new Dictionary<DateTime, List<ForecastSampleDTO>>(dataInfos.Count);

            IProductionFormula formula;
            foreach (var dataInfo in dataInfos)
            {
                List<ForecastSampleDTO> samples = new List<ForecastSampleDTO>(generators.Count);
                dto.ProductionForecastSamples.Add(dataInfo.CurrentTime, samples);
                foreach (var generator in generators)
                {
                    if (!productionFormulas.TryGetValue(generator.DMSType, out formula))
                    {
                        Logger.Instance.Log($"[{GetType().Name}] Cannot find formula for DMSType: {generator.DMSType}. Skipping calculation of entity with gid: {generator.GlobalId}.");
                        continue;
                    }

                    ForecastSampleDTO sample = new ForecastSampleDTO()
                    {
                        GeneratorGid = generator.GlobalId,
                        ActivePower = formula.CalculateProduction(generator, dataInfo)
                    };

                    samples.Add(sample);
                }
            }

            return dto;
        }

        private void InitializeProductionFormulas()
        {
            productionFormulas = new Dictionary<DMSType, IProductionFormula>()
            {
                { DMSType.WINDGENERATOR, new WindGeneratorProductionFormula() },
                { DMSType.SOLARGENERATOR, new SolarPanelProductionFormula() }
            };
        }
    }
}
