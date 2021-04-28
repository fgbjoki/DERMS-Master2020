namespace CalculationEngine.CommonComponents
{
    public interface ITopologyDependentComponent
    {
        void ProcessTopologyChanges();
        void ProcessAnalogChanges(long measurementGid, float newMeasurementValue);
    }
}