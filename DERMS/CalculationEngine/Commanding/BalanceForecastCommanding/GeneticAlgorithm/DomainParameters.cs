using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Fitness.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm
{
    public class DomainParameters
    {
        public EnergyBalanceFitnessParameters FitnessParameters { get; set; }

        /// <summary>
        /// Simulation interval used for calculating energy uses/losses.
        /// </summary>
        public ulong SimulationInterval { get; set; }

        /// <summary>
        /// Maximum seconds for algorithm to compute.
        /// </summary>
        public double CalculatingTime { get; set; }

        public uint PopulationSize { get; set; }

        public float EnergyDemand { get; set; }
    }
}
