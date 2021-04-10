namespace CalculationEngine.EnergyCalculators
{
    public interface IEnergyBalanceCalculator
    {
        void PerformCalculation();
        void Recalculate(long measurementGid, float newMeasurementValue);
    }
}