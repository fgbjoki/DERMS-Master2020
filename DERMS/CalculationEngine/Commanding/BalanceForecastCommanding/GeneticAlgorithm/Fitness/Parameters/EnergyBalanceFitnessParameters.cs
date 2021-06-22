namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Fitness.Parameters
{
    public class EnergyBalanceFitnessParameters
    {
        public float CostOfEnergyImportPerKWH { get; set; }
        public float CostOfEnergyStorageUsePerKWH { get; set; }
        public float CostOfGeneratorShutdownPerKWH { get; set; }
    }
}
