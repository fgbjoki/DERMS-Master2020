using CalculationEngine.Model.EnergyCalculations;
using CalculationEngine.TransactionProcessing.Storage.EnergyBalance;
using CalculationEngine.TransactionProcessing.Storage.Forecast;
using Common.ComponentStorage;
using Common.DataTransferObjects;
using Common.ServiceInterfaces.CalculationEngine;
using System;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.DataPreparation.Consumption
{
    public interface IConsumptionPreparation
    {
        float CalculateEnergyDemand(DateTime startingPoint, ulong forecastedMinutes, IWeatherForecastStorage weatherForecastStorage, ConsumptionForecastStorage consumptionForecastStorage);
    }
}
