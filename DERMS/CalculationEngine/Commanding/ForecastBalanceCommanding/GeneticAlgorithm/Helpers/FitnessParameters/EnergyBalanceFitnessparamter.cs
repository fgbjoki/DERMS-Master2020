namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Helpers.FitnessParameters
{
    public class EnergyBalanceFitnessparamter : BaseFitnessParameter
    {
        public float CostOfEnergyStorageUsePerKWH { get; set; }
        public float CostOfGeneratorShutDownPerKWH { get; set; }
        public float CostOfNetworkEnergyImportPerKWH { get; set; }

        public float EnergyDemand { get; set; }

        public ulong SimulationInterval { get; set; }
    }
}
