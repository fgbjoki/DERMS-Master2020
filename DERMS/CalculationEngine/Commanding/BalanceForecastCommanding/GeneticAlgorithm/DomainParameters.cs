using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Fitness.Parameters;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm
{
    public class DomainParameters
    {
        public EnergyBalanceFitnessParameters FitnessParameters { get; set; }

        /// <summary>
        /// Simulation interval used for calculating energy uses/losses. Time is represented in seconds.
        /// </summary>
        public ulong SimulationInterval { get; set; }

        /// <summary>
        /// Maximum seconds for algorithm to compute. Time is represented in seconds.
        /// </summary>
        public double CalculatingTime { get; set; }

        public uint PopulationSize { get; set; }

        public float EnergyDemand { get; set; }
    }
}
