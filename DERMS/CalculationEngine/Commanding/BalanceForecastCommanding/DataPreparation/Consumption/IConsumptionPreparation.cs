using System;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.DataPreparation.Consumption
{
    public interface IConsumptionPreparation
    {
        float CalculateEnergyDemand(DateTime startingPoint, int forecastedMinutes);
    }
}
