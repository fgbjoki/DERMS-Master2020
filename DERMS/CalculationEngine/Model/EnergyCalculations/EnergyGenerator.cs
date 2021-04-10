namespace CalculationEngine.Model.EnergyCalculations
{
    /// <summary>
    /// Represents anything that generates active power. (solar panel, wind generator, energy storage)
    /// </summary>
    public class EnergyGenerator : CalculationObject
    {
        public EnergyGenerator(long globalId) : base(globalId)
        {
        }
    }
}
