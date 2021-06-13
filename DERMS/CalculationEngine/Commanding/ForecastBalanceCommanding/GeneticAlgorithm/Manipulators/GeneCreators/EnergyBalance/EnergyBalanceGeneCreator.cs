using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.EnergyBalance.Genes;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.GeneCreators.EnergyBalance
{
    public abstract class EnergyBalanceGeneCreator<T> : BaseGeneCreator<T>
        where T : EnergyBalanceGene
    {
        public ulong SimulationInterval { get; set; }
    }
}
