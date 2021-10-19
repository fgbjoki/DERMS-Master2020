using CalculationEngine.Model.Forecast.ConsumptionForecast;
using Common.DataTransferObjects;
using Common.WeatherAPI;
using DERMS;
using System;

namespace CalculationEngine.Forecast.ConsumptionForecast.Formulas
{
    public class ConsumptionFormula : IConsumptionFormula
    {
        public float CalculateConsumption(Consumer consumer, WeatherDayData weatherData)
        {
            return 0;
        }
    }
}
