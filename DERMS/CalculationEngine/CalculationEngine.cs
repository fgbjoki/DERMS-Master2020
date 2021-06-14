using CalculationEngine.Graphs;
using CalculationEngine.Model.Topology.Graph;
using CalculationEngine.Schema;
using CalculationEngine.TransactionProcessing.Storage.Topology;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.PubSub;
using Common.ServiceInterfaces;
using Common.ServiceInterfaces.CalculationEngine;
using Common.ServiceInterfaces.Transaction;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using Common.DataTransferObjects.CalculationEngine;
using CalculationEngine.TransactionProcessing.Storage;
using CalculationEngine.PubSub.DynamicListeners;
using CalculationEngine.TransactionProcessing.Storage.EnergyBalance;
using CalculationEngine.EnergyCalculators;
using NServiceBus;
using Common.Helpers.Breakers;
using CalculationEngine.TransactionProcessing.Storage.DERStates;
using CalculationEngine.DERStates;
using CalculationEngine.Commanding.BreakerCommanding;
using CalculationEngine.Commanding.DERCommanding;
using CalculationEngine.TransactionProcessing.Storage.DERCommanding;
using CalculationEngine.DERStates.CommandScheduler;
using CalculationEngine.TransactionProcessing.Storage.Forecast;
using CalculationEngine.Forecast.ProductionForecast;
using CalculationEngine.Forecast.WeatherForecast;
using Common.WeatherAPI;
using Common.DataTransferObjects.CalculationEngine.DEROptimalCommanding;
using Common.ServiceInterfaces.CalculationEngine.DEROptimalCommanding;
using CalculationEngine.Commanding.DEROptimalCommanding;
using CalculationEngine.TransactionProcessing.Storage.EnergyImporter;
using Common.DataTransferObjects;
using CalculationEngine.Commanding.ForecastBalanceCommanding;

namespace CalculationEngine
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class CalculationEngine : ITransaction, IModelPromotionParticipant, ISchemaRepresentation, IBreakerCommanding, IDERStateDeterminator, IDERCommandingProcessor, IProductionForecast, IDEROptimalCommanding, IWeatherForecastStorage, IEnergyBalanceForecast
    {
        private readonly string serviceName = "Calculation Engine";
        private string serviceUrlForTransaction;

        private ModelResourcesDesc modelResourcesDesc;

        private GraphBranchManipulator graphManipulator;

        private BreakerMessageMapping breakerMessageMapping;

        private TopologyStorage topologyStorage;

        private EnergyBalanceStorage energyBalanceStorage;

        private TransactionManager transactionManager;

        private TopologyAnalysis.TopologyAnalysis topologyAnalysis;
        private DiscreteRemotePointStorage discreteRemotePointStorage;

        private DERStateStorage derStateStorage;

        private ProductionForecastStorage productionForecastStorage;

        private SchemaRepresentation schemaRepresentation;

        private DynamicPublisher dynamicPublisher;

        private DynamicListenersManager dynamicListenersManager;

        private EnergyBalanceCalculator energyBalanceCalculator;
        private DERStateController derStateDeterminator;

        private BreakerCommandingUnit breakerCommandingValidator;

        private DERCommandingProcessor derCommandingProcessor;
        private DERCommandingStorage derCommandingStorage;

        private EndpointConfiguration endpointConfiguration;

        private SchedulerCommandExecutor schedulerCommandExecutor;
        private ICommandScheduler commandScheduler;

        private IWeatherForecastStorage weatherForecastStorage;

        private IProductionForecast productionForecast;

        private IDEROptimalCommanding derOptimalCommanding;

        private IEnergyImporterProcessor energyImporter;
        private EnergyImproterStorage energyImporterStorage;

        private IEnergyBalanceForecast energyBalanceForecast;

        public CalculationEngine()
        {
            InternalCEInitialization();

            endpointConfiguration = InitializeDynamicPublisher();

            InitializeGraphs();

            InitializeStorages();

            InitializeForTransaction();

            InitializeCommandScheduler();

            energyImporter = new EnergyImporterProcessor(energyImporterStorage);
            energyBalanceCalculator = new EnergyBalanceCalculator(energyBalanceStorage, topologyAnalysis, energyImporter, dynamicPublisher);
            derStateDeterminator = new DERStateController(energyBalanceStorage.EnergySourceStorage, derStateStorage, topologyAnalysis, dynamicPublisher, schedulerCommandExecutor);
            derCommandingProcessor = new DERCommandingProcessor(derStateDeterminator, derCommandingStorage, schedulerCommandExecutor);
            productionForecast = new ProductionForecastCalculator(productionForecastStorage, weatherForecastStorage);
            derOptimalCommanding = new DEROptimalCommandingProcessor(derCommandingProcessor, derStateStorage, derCommandingStorage);

            energyBalanceForecast = new EnergyBalanceForecastProcessor(weatherForecastStorage, productionForecastStorage, derCommandingStorage);

            InitializePubSub();
        }

        private void InitializeGraphs()
        {
            discreteRemotePointStorage = new DiscreteRemotePointStorage();

            topologyAnalysis = new TopologyAnalysis.TopologyAnalysis(discreteRemotePointStorage, breakerMessageMapping);
            breakerCommandingValidator = new BreakerCommandingUnit(topologyAnalysis, discreteRemotePointStorage, breakerMessageMapping);
            topologyAnalysis.BreakerLoopCommandingValidator = breakerCommandingValidator;

            schemaRepresentation = new SchemaRepresentation();
        }

        private void InitializeCommandScheduler()
        {
            commandScheduler = new CommandScheduler();
            schedulerCommandExecutor = new SchedulerCommandExecutor();
            schedulerCommandExecutor.SetCommandScheduler(commandScheduler);
        }

        public bool Prepare()
        {
            return transactionManager.Prepare();
        }

        public bool Commit()
        {
            return transactionManager.Commit();
        }

        public bool Rollback()
        {
            return transactionManager.Rollback();
        }

        public bool ApplyChanges(List<long> insertedEntities, List<long> updatedEntities, List<long> deletedEntities)
        {
            return transactionManager.ApplyChanges(insertedEntities, updatedEntities, deletedEntities);
        }

        public IEnumerable<long> GetSchemaSources()
        {
            return schemaRepresentation.GetSchemaSources();
        }

        public SchemaGraphChanged GetSchema(long sourceId)
        {
            return schemaRepresentation.GetSchema(sourceId);
        }

        private void InternalCEInitialization()
        {
            LoadConfigurationFromAppConfig();

            breakerMessageMapping = new BreakerMessageMapping();
            graphManipulator = new GraphBranchManipulator();
            modelResourcesDesc = new ModelResourcesDesc();

            weatherForecastStorage = new WeatherForecastStorage(new WeatherApiClient("3b1ff7b44cbc4a7fa8d124540202911", "Novi Sad"));
        }

        private void InitializeStorages()
        {
            GraphsCreationProcessor graphsCreationProcessor = new GraphsCreationProcessor(modelResourcesDesc, discreteRemotePointStorage, schemaRepresentation, topologyAnalysis);
            topologyStorage = new TopologyStorage(breakerMessageMapping, graphManipulator, graphsCreationProcessor, modelResourcesDesc);

            energyBalanceStorage = new EnergyBalanceStorage();

            derStateStorage = new DERStateStorage();

            derCommandingStorage = new DERCommandingStorage();

            productionForecastStorage = new ProductionForecastStorage();

            energyImporterStorage = new EnergyImproterStorage();
        }

        private void InitializeForTransaction()
        {     
            transactionManager = new TransactionManager(serviceName, serviceUrlForTransaction);
            transactionManager.LoadTransactionProcessors(new List<ITransactionStorage>() { discreteRemotePointStorage, topologyStorage, energyBalanceStorage, derStateStorage, derCommandingStorage, productionForecastStorage, energyImporterStorage });
        }

        private EndpointConfiguration InitializeDynamicPublisher()
        {
            dynamicPublisher = new DynamicPublisher();
            return dynamicPublisher.ConfigureEndpointInstance(serviceName);
        }

        private void LoadConfigurationFromAppConfig()
        {
            ServicesSection serviceSection = ConfigurationManager.GetSection("system.serviceModel/services") as ServicesSection;
            ServiceEndpointElementCollection endpoints = serviceSection.Services[0].Endpoints;
            string transactionAddition = String.Empty;
            for (int i = 0; i < endpoints.Count; i++)
            {
                ServiceEndpointElement endpoint = endpoints[i];
                if (endpoint.Contract.Equals(typeof(ITransaction).ToString()))
                {
                    transactionAddition = $"/{endpoint.Address.OriginalString}";
                }
            }

            serviceUrlForTransaction = serviceSection.Services[0].Host.BaseAddresses[0].BaseAddress + transactionAddition;
        }

        private void InitializePubSub()
        {           
            InitializeDynamicListeners();
            InitializeDynamicHandlers();

            IEndpointInstance endpointInstance = dynamicListenersManager.StartListening(endpointConfiguration);
            dynamicPublisher.EndpointInstance = endpointInstance;
        }

        private void InitializeDynamicHandlers()
        {
            dynamicListenersManager.ConfigureSubscriptions(discreteRemotePointStorage.GetSubscriptions());
            dynamicListenersManager.ConfigureSubscriptions(topologyAnalysis.GetSubscriptions());
            dynamicListenersManager.ConfigureSubscriptions(energyBalanceCalculator.GetSubscriptions());
            dynamicListenersManager.ConfigureSubscriptions(derStateDeterminator.GetSubscriptions());
            dynamicListenersManager.ConfigureSubscriptions(derCommandingStorage.GetSubscriptions());
        }

        private void InitializeDynamicListeners()
        {
            dynamicListenersManager = new DynamicListenersManager("Calculation Engine");

            List<IDynamicListener> listeners = new List<IDynamicListener>()
            {
                new AnalogRemotePointChangedListener(),
                new DiscreteRemotePointChangedListener(),
            };

            foreach (var listener in listeners)
            {
                dynamicListenersManager.AddDynamicHandlers(listener.Topic, listener);
            }
        }

        public void UpdateBreakers()
        {
        }

        public bool ValidateCommand(long breakerGid, BreakerState breakerState)
        {
            return breakerCommandingValidator.ValidateCommand(breakerGid, breakerState);
        }

        public bool SendCommand(long breakerGid, int breakerValue)
        {
            return breakerCommandingValidator.SendCommand(breakerGid, breakerValue);
        }

        public bool IsEntityEnergized(long entityGid)
        {
            return derStateDeterminator.IsEntityEnergized(entityGid);
        }

        public CommandFeedback Command(long derGid, float commandingValue)
        {
            return derCommandingProcessor.Command(derGid, commandingValue);
        }

        public CommandFeedback ValidateCommand(long derGid, float commandingValue)
        {
            return derCommandingProcessor.ValidateCommand(derGid, commandingValue);
        }

        public ForecastDTO ForecastProductionMinutely(int minutes)
        {
            return productionForecast.ForecastProductionMinutely(minutes);
        }

        public ForecastDTO ForecastProductionHourly(int hours)
        {
            return productionForecast.ForecastProductionHourly(hours);
        }

        public DEROptimalCommandingFeedbackDTO CreateCommand(DEROptimalCommand command)
        {
            return derOptimalCommanding.CreateCommand(command);
        }

        public List<WeatherDataInfo> GetMinutesWeatherInfo(int minutes)
        {
            return weatherForecastStorage.GetMinutesWeatherInfo(minutes);
        }

        public List<WeatherDataInfo> GetHourlyWeatherInfo(int hours)
        {
            return weatherForecastStorage.GetHourlyWeatherInfo(hours);
        }

        public List<WeatherDataInfo> GetNextDayWeatherInfo()
        {
            return weatherForecastStorage.GetNextDayWeatherInfo();
        }

        public void Compute()
        {
            energyBalanceForecast.Compute();
        }
    }
}
