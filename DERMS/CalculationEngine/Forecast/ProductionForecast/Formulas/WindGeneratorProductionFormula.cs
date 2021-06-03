using CalculationEngine.Model.Forecast.ProductionForecast;
using CalculationEngine.Forecast.WeatherForecast;

namespace CalculationEngine.Forecast.ProductionForecast.Formulas
{
    public class WindGeneratorProductionFormula : ProductionFormula<WindGenerator>
    {
        protected override float CalculateProduction(WindGenerator generator, WeatherDataInfo weatherData)
        {
            if (weatherData.WindMPS >= generator.StartUpSpeed && weatherData.WindMPS < generator.NominalSpeed)
            {
                return (weatherData.WindMPS - generator.StartUpSpeed) * 0.035f * generator.NominalPower;
            }
            else
            {
                return generator.NominalPower;
            }
        }

        protected override bool IsWeatherDataOutOfBounds(WindGenerator generator, WeatherDataInfo weatherData)
        {
            return weatherData.WindMPS >= generator.CutOutSpeed || weatherData.WindMPS <= generator.StartUpSpeed;
        }
    }
}
