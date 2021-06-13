using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.Genes;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.Mutators.EnergyBalance
{
    public abstract class EnergyBalanceGeneMutator<T> : BaseGeneMutator<T>
        where T : Gene
    {
        /// <summary>
        /// Interval in seconds between two grid gene states.
        /// </summary>
        public ulong IntervalSimulation { get; set; }
    }
}
