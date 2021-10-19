using CalculationEngine.Model.Forecast.ConsumptionForecast;
using Common.DataTransferObjects;
using Common.WeatherAPI;

namespace CalculationEngine.Forecast.ConsumptionForecast.Formulas
{
    public interface IConsumptionFormula
    {
        float CalculateConsumption(Consumer consumer, WeatherDayData weatherData);
    }
}
