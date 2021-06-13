using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.EnergyBalance.Genes;
using System;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.Populations;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.Selectors.EnergyBalanceSelectors
{
    public class TopFitnessValueGenesSelector : BaseChromosomeSelector<GridStateGene>
    {
        public TopFitnessValueGenesSelector(float percentageSelection)
        {
            PercentageSelection = percentageSelection;
        }
        public float PercentageSelection { get; set; }

        public override void Select(Population<GridStateGene> sourcePopulation, Population<GridStateGene> destinationPopulation)
        {
            int chromosomesToPropagate = Convert.ToInt32(Math.Round(sourcePopulation.Chromosomes.Count * PercentageSelection));

            for (int i = 0; i < chromosomesToPropagate; i++)
            {
                destinationPopulation.Chromosomes.Add(sourcePopulation.Chromosomes[i]);
            }
        }
    }
}
