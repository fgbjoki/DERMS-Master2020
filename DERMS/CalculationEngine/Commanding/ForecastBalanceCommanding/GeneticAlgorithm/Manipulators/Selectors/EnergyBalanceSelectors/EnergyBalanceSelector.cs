using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.EnergyBalance.Genes;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.Populations;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.FitnessCalculators.EnergyBalance;
using System.Linq;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.Selectors.EnergyBalanceSelectors
{
    public class EnergyBalanceSelector : BaseChromosomeSelector<GridStateGene>
    {
        private GridStateFitnessCalculator gridStateFitnessCalculator;

        private TopFitnessValueGenesSelector topFitnessGenesSelector;
        private WorstFitnessValueGenesSelector worstFitnessGenesSelector;

        public EnergyBalanceSelector(GridStateFitnessCalculator gridStateFitnessCalculator)
        {
            this.gridStateFitnessCalculator = gridStateFitnessCalculator;

            topFitnessGenesSelector = new TopFitnessValueGenesSelector(0.2f);
            worstFitnessGenesSelector = new WorstFitnessValueGenesSelector(0.01f);
        }

        public override void Select(Population<GridStateGene> sourcePopulation, Population<GridStateGene> destinationPopulation)
        {
            foreach (var chromosome in sourcePopulation.Chromosomes)
            {
                gridStateFitnessCalculator.Calculate(chromosome);
            }

            sourcePopulation.Chromosomes = sourcePopulation.Chromosomes.OrderBy(x => x.FitnessValue).ToList();

            topFitnessGenesSelector.Select(sourcePopulation, destinationPopulation);
            worstFitnessGenesSelector.Select(sourcePopulation, destinationPopulation);
        }
    }
}
