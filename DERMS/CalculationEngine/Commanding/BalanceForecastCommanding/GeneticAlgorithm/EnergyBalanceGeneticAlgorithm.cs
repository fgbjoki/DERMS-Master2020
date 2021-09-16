using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model.Genes;
using System.Collections.Generic;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Manipulators.Selectors.Chromosome;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm
{
    public class EnergyBalanceGeneticAlgorithm : GeneticAlgorithm<DERGene>
    {
        private List<BaseChromosomeSelector> chromosomeSelectors;

        public EnergyBalanceGeneticAlgorithm(DomainParameters domainParameters) : base(domainParameters)
        {
            chromosomeSelectors = new List<BaseChromosomeSelector>()
            {
                new TopPercentageChromosomeSelector(),
                new WorstPercentageChromosomeSelector()
            };
        }

        protected override List<BaseChromosomeSelector> GetChromosomeSelectors()
        {
            return chromosomeSelectors;
        }
    }
}
