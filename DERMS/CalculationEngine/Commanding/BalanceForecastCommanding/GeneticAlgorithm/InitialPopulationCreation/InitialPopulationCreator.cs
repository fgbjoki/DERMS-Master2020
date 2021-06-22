using CalculationEngine.Commanding.BalanceForecastCommanding.DataPreparation.Production;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.InitialPopulationCreation.GeneCreators;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.InitialPopulationCreation.Model;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model.Genes;
using System.Collections.Generic;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.InitialPopulationCreation
{
    public class InitialPopulationCreator
    {
        private EnergyStorageInitialGeneCreator energyStorageGeneCreator;
        private GeneratorInitialGeneCreator generatorGeneCreator;

        public InitialPopulationCreator()
        {
            generatorGeneCreator = new GeneratorInitialGeneCreator();
        }

        public Population<DERGene> CreatePopulation(DomainParameters domainParameters, InitialPopulationCreationParameters modelParameters)
        {
            energyStorageGeneCreator = new EnergyStorageInitialGeneCreator(domainParameters);

            Population<DERGene> initialPopulation = new Population<DERGene>();

            var energyStorageEnumerator = modelParameters.EnergyStorages.GetEnumerator();

            for (int i = 0; i < domainParameters.PopulationSize; i++)
            {
                initialPopulation.Chromosomes.Add(new Chromosome<DERGene>());
            }

            PopulateChromosomes(initialPopulation, modelParameters, (int)domainParameters.PopulationSize);

            return initialPopulation;
        }

        private void PopulateChromosomes(Population<DERGene> population, InitialPopulationCreationParameters modelParameters, int populationSize)
        {
            PopulatePopulationWithGenerators(population, modelParameters.Generators, populationSize);
            PopulatePopulationWithEnergyStorages(population, modelParameters.EnergyStorages, populationSize);
        }

        private void PopulatePopulationWithGenerators(Population<DERGene> population, List<GeneratorProduction> generators, int populationSize)
        {
            foreach (var generator in generators)
            {
                var genes = generatorGeneCreator.CreateGene(generator, populationSize);

                for (int i = 0; i < population.Chromosomes.Count; ++i)
                {
                    var chromosome = population.Chromosomes[i];
                    chromosome.Genes.Add(genes[i]);
                }
            }
        }

        private void PopulatePopulationWithEnergyStorages(Population<DERGene> population, List<EnergyStorageEntity> energyStorages, int populationSize)
        {
            foreach (var energyStorage in energyStorages)
            {
                var genes = energyStorageGeneCreator.CreateGene(energyStorage, populationSize);

                for (int i = 0; i < population.Chromosomes.Count; ++i)
                {
                    var chromosome = population.Chromosomes[i];
                    chromosome.Genes.Add(genes[i]);
                }
            }
        }
    }
}
