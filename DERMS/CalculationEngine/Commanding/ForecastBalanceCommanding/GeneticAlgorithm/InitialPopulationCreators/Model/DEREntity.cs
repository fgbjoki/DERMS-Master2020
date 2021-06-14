namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.InitialPopulationCreators.Model
{
    public abstract class DEREntity
    {
        public long GlobalId { get; set; }
        public float NominalPower { get; set; }
        public float ActivePower { get; set; }
    }
}
