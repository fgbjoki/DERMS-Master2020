using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Fitness.Calculators.Chromosomes;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model.Genes;
using System.Linq;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Fitness
{
    public class FitnessCalculator
    {
        private EnergyBalanceFitnessCalculator energyBalanceFitnessCalculator;

        public FitnessCalculator()
        {
            energyBalanceFitnessCalculator = new EnergyBalanceFitnessCalculator();
        }

        public void Calculate<T>(Population<T> population, DomainParameters domainParameters) where T : Gene
        {
            foreach (var chromosome in population.Chromosomes)
            {
                energyBalanceFitnessCalculator.Calculate(chromosome as Chromosome<DERGene>, domainParameters);
            }

            population.Chromosomes.OrderBy(x => x.FitnessValue);
        }
    }
}
