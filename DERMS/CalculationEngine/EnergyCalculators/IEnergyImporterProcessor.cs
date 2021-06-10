namespace CalculationEngine.EnergyCalculators
{
    public interface IEnergyImporterProcessor
    {
        void ChangeSourceImportPower(long sourceGid, float activePower);
    }
}