using CalculationEngine.Commanding.BalanceForecastCommanding.DataPreparation.Consumption;
using CalculationEngine.Commanding.BalanceForecastCommanding.DataPreparation.Production;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Fitness.Parameters;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.InitialPopulationCreation;
using CalculationEngine.Model.DERCommanding;
using CalculationEngine.Model.Forecast.ProductionForecast;
using Common.ComponentStorage;
using Common.DataTransferObjects;
using Common.DataTransferObjects.CalculationEngine.EnergyBalanceForecast;
using Common.ServiceInterfaces.CalculationEngine;
using Common.UIDataTransferObject.EnergyBalanceForecast;
using System;
using System.Linq;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model.Genes;

namespace CalculationEngine.Commanding.BalanceForecastCommanding
{
    public class BalanceForecastCommandProcessor : IEnergyBalanceForecast
    {
        private GeneratorDataPreparator generatorPreparator;
        private EnergyStorageDataPreparator energyStoragePreparator;
        private IConsumptionPreparation consumptionPreparation;

        private InitialPopulationCreator initialPopulationCreator;

        private IWeatherForecastStorage weatherForecast;

        public BalanceForecastCommandProcessor(IWeatherForecastStorage weatherForecast, IStorage<Generator> generatorStorage, IStorage<DistributedEnergyResource> ders)
        {
            this.weatherForecast = weatherForecast;
            generatorPreparator = new GeneratorDataPreparator(generatorStorage);
            energyStoragePreparator = new EnergyStorageDataPreparator(ders);

            initialPopulationCreator = new InitialPopulationCreator();
        }

        public DERStateCommandingSequenceDTO Compute(DomainParametersDTO domainParameters)
        {
            WeatherDataInfo weatherData = GetWeahterData();
            var internalDomainParameters = CreateDomainParameters(domainParameters);

            internalDomainParameters.EnergyDemand = consumptionPreparation == null ? 0 : consumptionPreparation.CalculateEnergyDemand(DateTime.Now, domainParameters.SimulationInterval, weatherData);

            var initialPopulationParameters = CreateInputParameters(weatherData);

            var population = initialPopulationCreator.CreatePopulation(internalDomainParameters, initialPopulationParameters);

            var energyBalanceGeneticAlgorithm = new EnergyBalanceGeneticAlgorithm(internalDomainParameters);

            var bestResult = energyBalanceGeneticAlgorithm.Compute(population);

            return ConvertResults(bestResult);
        }

        private DERStateCommandingSequenceDTO ConvertResults(Chromosome<DERGene> bestResult)
        {
            DERStateCommandingSequenceDTO commandingSequence = new DERStateCommandingSequenceDTO();

            foreach (var gene in bestResult.Genes)
            {
                var derState = new Common.DataTransferObjects.CalculationEngine.EnergyBalanceForecast.DERStateDTO()
                {
                    GlobalId = gene.GlobalId,
                    ActivePower = gene.ActivePower,
                };

                if (gene.DMSType == Common.AbstractModel.DMSType.SOLARGENERATOR || gene.DMSType == Common.AbstractModel.DMSType.WINDGENERATOR)
                {
                    derState.IsEnergized = ((GeneratorGene)gene).IsEnergized;
                }
                else
                {
                    derState.IsEnergized = true;
                }

                commandingSequence.SuggestedDTOState.Add(derState);
                commandingSequence.ImportedEnergy = bestResult.ImportedEnergy;
            }

            return commandingSequence;
        }

        private InitialPopulationCreationParameters CreateInputParameters(WeatherDataInfo weatherDataInfo)
        {
            InitialPopulationCreationParameters inputParameters = new InitialPopulationCreationParameters();
            inputParameters.Generators = generatorPreparator.GenerateData(weatherDataInfo);
            inputParameters.EnergyStorages = energyStoragePreparator.CreateEntities();
            
            return inputParameters;
        }

        private WeatherDataInfo GetWeahterData()
        {
            return weatherForecast.GetMinutesWeatherInfo(1).First();
        }

        private DomainParameters CreateDomainParameters(DomainParametersDTO domainParameters)
        {
            return new DomainParameters()
            {
                FitnessParameters = new EnergyBalanceFitnessParameters()
                {
                    CostOfEnergyImportPerKWH = domainParameters.CostOfImportedEnergyPerKWH,
                    CostOfEnergyStorageUsePerKWH = domainParameters.CostOfEnergyStorageUsePerKWH,
                    CostOfGeneratorShutdownPerKWH = domainParameters.CostOfGeneratorShutdownPerKWH
                },
                PopulationSize = 1000,
                CalculatingTime = domainParameters.CalculatingTime,
                SimulationInterval = domainParameters.SimulationInterval
            };
        }
    }
}
