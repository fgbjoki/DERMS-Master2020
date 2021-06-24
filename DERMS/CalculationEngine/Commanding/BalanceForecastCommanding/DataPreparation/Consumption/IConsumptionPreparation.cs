using Common.DataTransferObjects;
using System;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.DataPreparation.Consumption
{
    public interface IConsumptionPreparation
    {
        float CalculateEnergyDemand(DateTime startingPoint, ulong forecastedMinutes, WeatherDataInfo weatherData);
    }
}
