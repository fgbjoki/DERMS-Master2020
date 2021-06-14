using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Helpers.FitnessParameters;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.InitialPopulationCreators.GeneCreators;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.Chromosomes;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.EnergyBalance.Genes;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.Populations;
using System.Collections.Generic;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.InitialPopulationCreators.Model;
using CalculationEngine.Commanding.ForecastBalanceCommanding.Production;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.InitialPopulationCreators
{
    public class GridStateInitialPopulationCreator
    {
        private readonly int simulationIteratrions = 4 * 24;

        private EnergyStorageInitialGeneCreator energyStorageGeneCreator;
        private GeneratorInitialGeneCreator generatorGeneCreator;

        public GridStateInitialPopulationCreator()
        {
            generatorGeneCreator = new GeneratorInitialGeneCreator();
        }

        public Population<GridStateGene> CreatePopulation(BoundaryParameteres boundaryParameters, InputParameters modelParameters)
        {
            energyStorageGeneCreator = new EnergyStorageInitialGeneCreator(boundaryParameters);

            Population<GridStateGene> initialPopulation = new Population<GridStateGene>();

            var energyStorageEnumerator = modelParameters.EnergyStorages.GetEnumerator();

            for (int i = 0; i < boundaryParameters.PopulationSize; i++)
            {
                initialPopulation.Chromosomes.Add(CreateChromosome(modelParameters));
            }

            PopulatePopulationWithGenerators(initialPopulation, modelParameters.Generators, boundaryParameters.PopulationSize);
            PopulatePopulationWithEnergyStorages(initialPopulation, modelParameters.EnergyStorages, simulationIteratrions);

            return initialPopulation;
        }

        private Chromosome<GridStateGene> CreateChromosome(InputParameters modelParameters)
        {
            Chromosome<GridStateGene> newChromosome = new Chromosome<GridStateGene>();

            for (int j = 0; j < simulationIteratrions; j++)
            {
                GridStateGene newGene = new GridStateGene();
                newGene.EnergyDemand = modelParameters.EnergyDemand[j];

                newChromosome.Genes.Add(newGene);
            }

            return newChromosome;
        }

        private void PopulatePopulationWithGenerators(Population<GridStateGene> population, List<ProductionForecast> productionForecast, int numberOfGenes)
        {
            for (int i = 0; i < productionForecast.Count; ++i)
            {
                foreach (var production in productionForecast[i].GeneratorProductions)
                {
                    List<GeneratorGene> generatorGenes = generatorGeneCreator.CreateGene(production, numberOfGenes);
                    
                    for (int j = 0; j < population.Chromosomes.Count; ++j)
                    {
                        var chromosome = population.Chromosomes[j];
                        chromosome.Genes[i].DERGenes.Add(generatorGenes[j]);

                    }
                }
            }
        }

        private void PopulatePopulationWithEnergyStorages(Population<GridStateGene> population, List<EnergyStorageEntity> energyStorages, int numberOfGenes)
        {
            foreach (var chromosome in population.Chromosomes)
            {
                foreach (var energyStorage in energyStorages)
                {
                    var energyStorageGenes = energyStorageGeneCreator.CreateGene(energyStorage, numberOfGenes);
                    for (int i = 0; i < chromosome.Genes.Count; ++i)
                    {
                        var gene = chromosome.Genes[i];
                        gene.DERGenes.Add(energyStorageGenes[i]);
                    }
                }
            }
        }
    }
}
