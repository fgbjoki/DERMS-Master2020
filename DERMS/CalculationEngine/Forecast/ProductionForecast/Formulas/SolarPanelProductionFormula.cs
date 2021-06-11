using CalculationEngine.Model.Forecast.ProductionForecast;
using Common.DataTransferObjects;

namespace CalculationEngine.Forecast.ProductionForecast.Formulas
{
    public class SolarPanelProductionFormula : ProductionFormula<SolarPanel>
    {
        protected override float CalculateProduction(SolarPanel generator, WeatherDataInfo weatherData)
        {
            float solarInsolation = 990 * (1 - weatherData.CloudCover/100);
            float cellTemperature = weatherData.TemperatureC + 0.025f * solarInsolation;
            return generator.NominalPower * solarInsolation * 0.00095f * (1 - 0.005f * (cellTemperature - 25));
        }

        protected override bool IsWeatherDataOutOfBounds(SolarPanel generator, WeatherDataInfo weatherData)
        {
            return !weatherData.Daylight;
        }
    }
}