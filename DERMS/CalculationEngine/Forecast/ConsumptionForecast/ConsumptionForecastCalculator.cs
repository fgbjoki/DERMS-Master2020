using CalculationEngine.Forecast.ConsumptionForecast.Formulas;
using CalculationEngine.Model.Forecast.ConsumptionForecast;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.DataTransferObjects;
using Common.DataTransferObjects.CalculationEngine;
using Common.Logger;
using Common.ServiceInterfaces.CalculationEngine;
using System.Collections.Generic;

namespace CalculationEngine.Forecast.ConsumptionForecast
{
    public class ConsumptionForecastCalculator : IConsumptionForecast
    {
        private IStorage<Consumer> storage;
        private Dictionary<DMSType, IConsumptionFormula> consumptionFormulas;
        private IWeatherForecastStorage weatherForecast;

        public ConsumptionForecastCalculator(IStorage<Consumer> storage, IWeatherForecastStorage weatherForecast)
        {
            this.weatherForecast = weatherForecast;
            this.storage = storage;

            InitializeConsumptionFormulas();
        }

        public ForecastDTO ForecastConsumptionHourly(int hours)
        {
            List<WeatherDataInfo> dataInfos = weatherForecast.GetHourlyWeatherInfo(hours);
            return CalculateConsumption(dataInfos);
        }

        private ForecastDTO CalculateConsumption(List<WeatherDataInfo> dataInfos)
        {
            List<Consumer> consumers = storage.GetAllEntities();

            ForecastDTO dto = new ForecastDTO();
            dto.ConsumptionForecastSamples = new Dictionary<System.DateTime, List<ForecastSampleDTO>>(dataInfos.Count);

            IConsumptionFormula formula;
            foreach(var dataInfo in dataInfos)
            {
                List<ForecastSampleDTO> samples = new List<ForecastSampleDTO>(consumers.Count);
                dto.ConsumptionForecastSamples.Add(dataInfo.CurrentTime, samples);
                foreach(var consumer in consumers)
                {
                    if(!consumptionFormulas.TryGetValue(consumer.DMSType, out formula))
                    {
                        Logger.Instance.Log($"[{GetType().Name}] Cannot find formula for DMSType: {consumer.DMSType}. Skipping calculation of entity with gid: {consumer.GlobalId}.");
                        continue;
                    }

                    ForecastSampleDTO sample = new ForecastSampleDTO()
                    {
                        GeneratorGid = consumer.GlobalId,
                        //ActivePower = formula.CalculateConsumption(consumer, dataInfo)
                    };

                    samples.Add(sample);
                }
            }
            
            return dto;
        }

        private void InitializeConsumptionFormulas()
        {
            consumptionFormulas = new Dictionary<DMSType, IConsumptionFormula>()
            {
                { DMSType.ENERGYCONSUMER, new ConsumptionFormula() }
            };
        }
    }
}
