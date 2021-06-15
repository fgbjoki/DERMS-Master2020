namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Helpers.FitnessParameters
{
    public class BoundaryParameteres : BaseFitnessParameter
    {
        public BoundaryParameteres()
        {

        }

        public int PopulationSize { get; set; }

        public float CostOfEnergyStorageUsePerKWH { get; set; }
        public float CostOfGeneratorShutDownPerKWH { get; set; }
        public float CostOfNetworkEnergyImportPerKWH { get; set; }

        public float EnergyDemand { get; set; }

        public ulong SimulationInterval { get; set; }

        public float LowerBoundStateOfCharge { get; set; }
        public float UpperBoundStateOfCharge { get; set; }

        public void LoadNextTimestampData()
        {

        }
    }
}
