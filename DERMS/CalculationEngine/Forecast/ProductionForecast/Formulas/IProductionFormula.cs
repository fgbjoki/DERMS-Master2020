using CalculationEngine.Model.Forecast.ProductionForecast;
using Common.DataTransferObjects;

namespace CalculationEngine.Forecast.ProductionForecast.Formulas
{
    public interface IProductionFormula
    {
        float CalculateProduction(Generator generator, WeatherDataInfo weatherData);
    }
}