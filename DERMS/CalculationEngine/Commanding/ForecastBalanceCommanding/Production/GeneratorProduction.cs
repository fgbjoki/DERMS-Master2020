namespace CalculationEngine.Commanding.ForecastBalanceCommanding.Production
{
    public class GeneratorProduction
    {
        public GeneratorProduction(long globalId, float activePower)
        {
            GlobalId = globalId;
            ActivePower = activePower;
        }

        public long GlobalId { get; private set; }

        public float ActivePower { get; set; }
    }
}
