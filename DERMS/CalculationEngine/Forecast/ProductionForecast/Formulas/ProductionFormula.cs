using CalculationEngine.Forecast.WeatherForecast;
using CalculationEngine.Model.Forecast.ProductionForecast;
using Common.DataTransferObjects;

namespace CalculationEngine.Forecast.ProductionForecast.Formulas
{
    public abstract class ProductionFormula<T> : IProductionFormula
        where T : Generator
    {
        public float CalculateProduction(Generator generator, WeatherDataInfo weatherData)
        {
            T entity = generator as T;
            if (entity == null)
            {
                return 0;
            }

            if (IsWeatherDataOutOfBounds(entity, weatherData))
            {
                return 0f;
            }

            return CalculateProduction(entity, weatherData);
        }

        protected abstract float CalculateProduction(T generator, WeatherDataInfo weatherData);

        protected abstract bool IsWeatherDataOutOfBounds(T generator, WeatherDataInfo weatherData);
    }
}
