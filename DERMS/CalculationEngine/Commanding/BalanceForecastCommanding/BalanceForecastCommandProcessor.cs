using CalculationEngine.Commanding.BalanceForecastCommanding.DataPreparation.Consumption;
using CalculationEngine.Commanding.BalanceForecastCommanding.DataPreparation.Production;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.InitialPopulationCreation;
using CalculationEngine.Model.DERCommanding;
using CalculationEngine.Model.Forecast.ProductionForecast;
using Common.ComponentStorage;
using Common.ServiceInterfaces.CalculationEngine;
using System;

namespace CalculationEngine.Commanding.BalanceForecastCommanding
{
    public class BalanceForecastCommandProcessor
    {
        private GeneratorDataPreparator generatorPreparator;
        private EnergyStorageDataPreparator energyStoragePreparator;
        private IConsumptionPreparation consumptionPreparation;

        private InitialPopulationCreator initialPopulationCreator;

        public BalanceForecastCommandProcessor(IWeatherForecastStorage weatherForecast, IStorage<Generator> generatorStorage, IStorage<DistributedEnergyResource> ders)
        {
            generatorPreparator = new GeneratorDataPreparator(weatherForecast, generatorStorage);
            energyStoragePreparator = new EnergyStorageDataPreparator(ders);

            initialPopulationCreator = new InitialPopulationCreator();
        }

        public void Compute(int forecastMinutes = 15)
        {
            var domainParameters = CreateDefaultBoundaryParameters((ulong)forecastMinutes);

            domainParameters.EnergyDemand = consumptionPreparation == null ? 0 : consumptionPreparation.CalculateEnergyDemand(DateTime.Now, forecastMinutes);

            var initialPopulationParameters = CreateInputParameters();

            var population = initialPopulationCreator.CreatePopulation(domainParameters, initialPopulationParameters);

            var energyBalanceGeneticAlgorithm = new EnergyBalanceGeneticAlgorithm(domainParameters);
            var bestResult = energyBalanceGeneticAlgorithm.Compute(population);

        }

        private InitialPopulationCreationParameters CreateInputParameters()
        {
            InitialPopulationCreationParameters inputParameters = new InitialPopulationCreationParameters();
            inputParameters.Generators = generatorPreparator.GenerateData();
            inputParameters.EnergyStorages = energyStoragePreparator.CreateEntities();
            
            return inputParameters;
        }

        private DomainParameters CreateDefaultBoundaryParameters(ulong forecastedMinutes)
        {
            return new DomainParameters()
            {
                FitnessParameters  = new GeneticAlgorithm.Fitness.Parameters.EnergyBalanceFitnessParameters()
                {
                    CostOfEnergyImportPerKWH = 0.5f,
                    CostOfEnergyStorageUsePerKWH = 0.05f,
                    CostOfGeneratorShutdownPerKWH = 1f
                },
                SimulationInterval = forecastedMinutes * 60, // 15min
                PopulationSize = 1000,
            };
        }
    }
}
