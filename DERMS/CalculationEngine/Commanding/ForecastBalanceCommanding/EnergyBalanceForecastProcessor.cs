using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Helpers.FitnessParameters;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.InitialPopulationCreators;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Parameters;
using CalculationEngine.Commanding.ForecastBalanceCommanding.ModelAggeragation;
using CalculationEngine.Commanding.ForecastBalanceCommanding.Production;
using CalculationEngine.Model.DERCommanding;
using CalculationEngine.Model.Forecast.ProductionForecast;
using Common.ComponentStorage;
using Common.ServiceInterfaces.CalculationEngine;
using System.Collections.Generic;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding
{
    public class EnergyBalanceForecastProcessor : IEnergyBalanceForecast
    {
        private ProductionDataAggregator productionDataAggregator;

        private EnergyStorageAggregator energyStorageModelCreator;

        private EnergyBalanceGeneticAlgorithm energyBalanceGeneticAlgorithm;

        private GridStateInitialPopulationCreator initialPopulationCreator;

        public EnergyBalanceForecastProcessor(IWeatherForecastStorage weatherForecast, IStorage<Generator> generatorStorage, IStorage<DistributedEnergyResource> ders)
        {
            energyStorageModelCreator = new EnergyStorageAggregator(ders);
            productionDataAggregator = new ProductionDataAggregator(weatherForecast, generatorStorage);
            initialPopulationCreator = new GridStateInitialPopulationCreator();
        }

        public void Compute()
        {
            var boundaryParameters = CreateDefaultBoundaryParameters();

            var population = initialPopulationCreator.CreatePopulation(boundaryParameters, CreateInputParameters(15 * 4 * 24));
            
            //energyBalanceGeneticAlgorithm = new EnergyBalanceGeneticAlgorithm(new EnergyBalanceGeneticAlgorithmParameters(boundaryParameters));
            //energyBalanceGeneticAlgorithm.Compute(population);
        }

        private InputParameters CreateInputParameters(int numberOfIterations)
        {
            InputParameters inputParameters = new InputParameters();
            inputParameters.Generators = productionDataAggregator.GenerateData(15);
            inputParameters.EnergyStorages = energyStorageModelCreator.CreateEntities();

            // TODO REMOVE THIS
            inputParameters.EnergyDemand = new List<float>();
            for (int i = 0; i < numberOfIterations; i++)
            {
                inputParameters.EnergyDemand.Add(0);
            }

            return inputParameters;
        }

        private BoundaryParameteres CreateDefaultBoundaryParameters()
        {
            return new BoundaryParameteres()
            {
                CostOfEnergyStorageUsePerKWH = 0.05f,
                CostOfGeneratorShutDownPerKWH = 1f,
                CostOfNetworkEnergyImportPerKWH = 0.5f,
                LowerBoundStateOfCharge = 0.2f,
                UpperBoundStateOfCharge = 1f,
                SimulationInterval = 15 * 60,
                PopulationSize = 1000,
            };
        }
    }
}
