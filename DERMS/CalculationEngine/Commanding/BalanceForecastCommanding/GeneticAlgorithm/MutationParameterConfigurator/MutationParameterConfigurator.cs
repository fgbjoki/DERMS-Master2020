using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model.Genes;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.MutationParameterConfigurator
{
    public class MutationParameterConfigurator
    {
        public void ConfigureMutationRate<T>(Population<T> population, GeneticAlgorithmParameters gaParameters)
            where T : Gene
        {
            float chromosomePercentageWithInvalidFitness = CalculateFaultChromosomePercentage(population);

            if (chromosomePercentageWithInvalidFitness > 0.99)
            {
                gaParameters.MutationRate = 1f;
            }
            else if (chromosomePercentageWithInvalidFitness > 0.8)
            {
                gaParameters.MutationRate = 0.4f;
            }
            else if (chromosomePercentageWithInvalidFitness > 0.6)
            {
                gaParameters.MutationRate = 0.1f;
            }
            else
            {
                gaParameters.MutationRate = 0.01f;
            }
        }

        private float CalculateFaultChromosomePercentage<T>(Population<T> population) where T : Gene
        {
            int faultedChromosomes = 0;

            foreach (var chromosome in population.Chromosomes)
            {
                if (float.IsInfinity(chromosome.FitnessValue))
                {
                    faultedChromosomes++;
                }
            }

            return (float)faultedChromosomes / population.Chromosomes.Count;
        }
    }
}
