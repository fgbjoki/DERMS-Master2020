using CalculationEngine.Forecast.WeatherForecast;
using CalculationEngine.Model.Forecast.ProductionForecast;

namespace CalculationEngine.Forecast.ProductionForecast.Formulas
{
    public interface IProductionFormula
    {
        float CalculateProduction(Generator generator, WeatherDataInfo weatherData);
    }
}