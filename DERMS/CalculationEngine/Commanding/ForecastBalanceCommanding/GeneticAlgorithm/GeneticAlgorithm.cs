using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.ChromosomeCreators;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.CrossoverManipulators;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.Mutators;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.Selectors;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.Chromosomes;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.Genes;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.Populations;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Parameters;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Parameters.Convergation;
using System.Timers;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm
{
    public abstract class GeneticAlgorithm<T, TT>
        where T : Gene, new()
        where TT : BaseGeneticAlgorithmParameters<T>
    {
        protected TT parameters;

        private ConvergationParameters convergationParameters;

        private CrossoverManipulator crossoverManipulator;
        private Mutator mutator;

        private object locker;

        private Timer algorithmStopper;
        private bool shouldContinue;

        public GeneticAlgorithm(TT parameters)
        {
            locker = new object();
            algorithmStopper = new Timer();

            this.parameters = parameters;
            convergationParameters = new ConvergationParameters();
            crossoverManipulator = new CrossoverManipulator();
            mutator = new Mutator();
        }

        public Chromosome<T> Compute(Population<T> currentPopulation)
        {
            shouldContinue = true;
            SetUpTimer();

            for (int iteration = 0; shouldContinue; ++iteration)
            {
                // TODO REMOVE THIS
                System.Console.WriteLine($"Iteration= {iteration}");
                currentPopulation = Selection(currentPopulation);

                Crossover(currentPopulation);

                Mutation(currentPopulation);
            }

            return currentPopulation.GetBestChromosome();
        }

        protected abstract BaseChromosomeSelector<T> GetSelector();

        protected abstract BaseChromosomeCreator<T> GetChromosomeCreator();

        protected abstract BaseGeneMutator<T> GetGeneMutator();

        protected virtual void PopulateFitnessParameters()
        {

        }

        private Population<T> Selection(Population<T> population)
        {
            Population<T> newPopulation = new Population<T>();

            BaseChromosomeSelector<T> selector = GetSelector();
            PopulateFitnessParameters();

            selector.Select(population, newPopulation);

            return newPopulation;
        }

        private void Mutation(Population<T> currentPopulation)
        {
            BaseGeneMutator<T> geneMutator = GetGeneMutator();
            mutator.Mutate(geneMutator, currentPopulation, convergationParameters.MutationProbability);
        }

        private void Crossover(Population<T> currentPopulation)
        {
            BaseChromosomeCreator<T> chromosomeCreator = GetChromosomeCreator();
            crossoverManipulator.Crossover(chromosomeCreator, currentPopulation, parameters.PopulationSize);
        }

        private void SetUpTimer()
        {
            algorithmStopper.AutoReset = false;
            algorithmStopper.Interval = parameters.ComputingTimeLimit * 1000;
            algorithmStopper.Elapsed += StopTheAlgorithm;
            algorithmStopper.Enabled = true;
        }

        private void StopTheAlgorithm(object sender, ElapsedEventArgs e)
        {
            algorithmStopper.Enabled = false;
            shouldContinue = false;
        }
    }
}
