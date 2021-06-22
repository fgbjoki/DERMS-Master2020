using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Fitness;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Helpers;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Manipulators.Crossovers;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Manipulators.Mutators;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Manipulators.Selectors;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Manipulators.Selectors.Chromosome;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model.Genes;
using System.Collections.Generic;
using System.Timers;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm
{
    public abstract class GeneticAlgorithm<T>
        where T : DERGene
    {
        private DomainParameters domainParameters;
        private Timer algorithmStopper;

        private Mutator mutator;
        private Selector selector;
        private FitnessCalculator fitnessCalculator;
        private PopulationCrossover populationCrossover;

        private GeneticAlgorithmParameters gaParameters;

        private MutationParameterConfigurator.MutationParameterConfigurator mutationParameterConfigurator;

        private bool shouldCompute = true;

        public GeneticAlgorithm(DomainParameters domainParameters)
        {
            this.domainParameters = domainParameters;

            selector = new Selector();
            fitnessCalculator = new FitnessCalculator();
            gaParameters = new GeneticAlgorithmParameters() { MutationRate = 1f };

            EnergyStorageActivePowerCalculator esAPCalculator = new EnergyStorageActivePowerCalculator()
            {
                SimulationInterval = domainParameters.SimulationInterval
            };

            populationCrossover = new PopulationCrossover(esAPCalculator);
            mutator = new Mutator(esAPCalculator);

            mutationParameterConfigurator = new MutationParameterConfigurator.MutationParameterConfigurator();
        }

        public Chromosome<T> Compute(Population<T> initialPopulation)
        {
            SetUpTimer();

            Population<T> currentPopulation = initialPopulation;
            for (int iteration = 0; shouldCompute; ++iteration)
            {
                // TODO REMOVE THIS
                System.Console.WriteLine($"Iteration= {iteration}");

                FitnessCalculation(currentPopulation);

                MutationParameterConfiguration(currentPopulation);

                currentPopulation = Selection(currentPopulation);

                Crossover(currentPopulation);

                Mutation(currentPopulation);
            }

            FitnessCalculation(currentPopulation);

            return currentPopulation.Chromosomes[0];
        }

        protected abstract List<BaseChromosomeSelector> GetChromosomeSelectors();

        private Population<T> Selection(Population<T> population)
        {
            var chromosomeSelectors = GetChromosomeSelectors();
            return selector.Select(population, chromosomeSelectors);
        }

        private void FitnessCalculation(Population<T> population)
        {
            fitnessCalculator.Calculate(population, domainParameters);
        }

        private void Crossover(Population<T> population)
        {
            populationCrossover.Crossover(population, (int)domainParameters.PopulationSize);
        }

        private void Mutation(Population<T> population)
        {
            MutationParameterConfiguration(population);
            mutator.Mutate(population, gaParameters);
        }

        private void MutationParameterConfiguration(Population<T> population)
        {
            mutationParameterConfigurator.ConfigureMutationRate(population, gaParameters);
        }

        private void SetUpTimer()
        {
            algorithmStopper.AutoReset = false;
            algorithmStopper.Interval = domainParameters.CalculatingTime * 1000;
            algorithmStopper.Elapsed += StopTheAlgorithm;
            algorithmStopper.Enabled = true;
        }

        private void StopTheAlgorithm(object sender, ElapsedEventArgs e)
        {
            algorithmStopper.Enabled = false;
            shouldCompute = false;
        }
    }
}
