using Common.ComponentStorage;
using Common.ServiceInterfaces;
using Common.ServiceInterfaces.Transaction;
using Common.ServiceInterfaces.UIAdapter.SummaryJobs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using UIAdapter.TransactionProcessing.Storages;
using Common.UIDataTransferObject.RemotePoints;
using UIAdapter.SummaryJobs;
using Common.PubSub;
using UIAdapter.PubSub.DynamicListeners;
using UIAdapter.Schema;
using UIAdapter.TransactionProcessing.Storages.Schema;
using Common.ServiceInterfaces.UIAdapter;
using Common.UIDataTransferObject.Schema;
using NServiceBus;
using Common.UIDataTransferObject.DERGroup;
using UIAdapter.Commanding;
using Common.Helpers.Breakers;
using Common.DataTransferObjects;
using UIAdapter.TransactionProcessing.Storages.NetworkModel;
using UIAdapter.SummaryJobs.NetworkModelSummary;
using Common.UIDataTransferObject.NetworkModel;
using Common.UIDataTransferObject.Forecast.Production;
using UIAdapter.Forecast.Production;
using Common.AbstractModel;
using Common.UIDataTransferObject.DEROptimalCommanding;
using UIAdapter.Commanding.DEROptimalCommanding;
using UIAdapter.Forecast.Weather;

namespace UIAdapter
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class UIAdapter : ITransaction, IModelPromotionParticipant, IAnalogRemotePointSummaryJob, IDiscreteRemotePointSummaryJob, ISchema, IDERGroupSummaryJob, IBreakerCommanding, IDERCommanding, INetworkModelSummaryJob, IProductionForecast, IDEROptimalCommandingProxy, IWeatherForecast
    {
        private readonly string serviceName = "UIAdapter";
        private string serviceUrlForTransaction;

        private BreakerMessageMapping breakerMessageMapping = new BreakerMessageMapping();

        private TransactionManager transactionManager;

        private EnergySourceStorage schemaEnergySourceStorage;
        private BreakerStorage schemaBreakerStorage;

        private AnalogRemotePointStorage analogRemotePointStorage;
        private DiscreteRemotePointStorage discreteRemotePointStorage;

        private NetworkModelStorage networkModelStorage;

        private TransactionProcessing.Storages.DERGroup.DERGroupStorage derGroupStorage;

        private DERGroupSummaryJob derGroupSummaryJob;
        private AnalogRemotePointSummaryJob analogRemotePointSummaryJob;
        private DiscreteRemotePointSummaryJob discreteRemotePointSummaryJob;
        private NetworkModelSummaryJob networkModelSummaryJob;

        private DynamicListenersManager dynamicListenerManager;

        private SchemaRepresentation schemaRepresentation;

        private IBreakerCommanding breakerCommanding;
        private IDERCommanding derCommanding;

        private IWeatherForecast weatherForecast;
        private IProductionForecast productionForecast;
        private IDEROptimalCommandingProxy derOptimalCommandingProxy;

        public UIAdapter()
        {
            LoadConfigurationFromAppConfig();

            transactionManager = new TransactionManager(serviceName, serviceUrlForTransaction);
            breakerCommanding = new BreakerCommandingProxy(breakerMessageMapping);
            derCommanding = new DERCommandingProxy();
            weatherForecast = new WeatherForecastProxy();
            productionForecast = new ProductionForecastAggregator();

            InitializeTransactionStorages();

            InitializeSchemaRepresentation();
            derOptimalCommandingProxy = new DEROptimalCommandingProxy(derCommanding, networkModelStorage, derGroupStorage);

            InitializeJobs();

            InitializePubSub();
        }

        private void InitializeSchemaRepresentation()
        {
            schemaRepresentation = new SchemaRepresentation(schemaEnergySourceStorage, schemaBreakerStorage);
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
        

        private void InitializeJobs()
        {
            derGroupSummaryJob = new DERGroupSummaryJob(derGroupStorage);
            analogRemotePointSummaryJob = new AnalogRemotePointSummaryJob(analogRemotePointStorage);
            discreteRemotePointSummaryJob = new DiscreteRemotePointSummaryJob(discreteRemotePointStorage);
            networkModelSummaryJob = new NetworkModelSummaryJob(networkModelStorage);
        }

        private void InitializeDynamicHandlers()
        {
            dynamicListenerManager.ConfigureSubscriptions(derGroupStorage.GetSubscriptions());
            dynamicListenerManager.ConfigureSubscriptions(analogRemotePointStorage.GetSubscriptions());
            dynamicListenerManager.ConfigureSubscriptions(discreteRemotePointStorage.GetSubscriptions());
            dynamicListenerManager.ConfigureSubscriptions(schemaRepresentation.GetSubscriptions());
        }

        private void InitializeDynamicListeners()
        {
            dynamicListenerManager = new DynamicListenersManager(serviceName);
            List<IDynamicListener> listeners = new List<IDynamicListener>()
            {
                new AnalogRemotePointChangedListener(),
                new DiscreteRemotePointChangedListener(),
                new DERStateChangedListener(),
                new EnergyBalanceChangedListener()
            };

            foreach (var listener in listeners)
            {
                dynamicListenerManager.AddDynamicHandlers(listener.Topic, listener);
            }
        }

        private void InitializePubSub()
        {
            InitializeDynamicListeners();
            InitializeDynamicHandlers();

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration(serviceName);
            dynamicListenerManager.StartListening(endpointConfiguration);
        }

        private void InitializeTransactionStorages()
        {
            networkModelStorage = new NetworkModelStorage();

            analogRemotePointStorage = new AnalogRemotePointStorage();
            discreteRemotePointStorage = new DiscreteRemotePointStorage();

            schemaEnergySourceStorage = new EnergySourceStorage();
            schemaBreakerStorage = new BreakerStorage();

            derGroupStorage = new TransactionProcessing.Storages.DERGroup.DERGroupStorage();

            transactionManager.LoadTransactionProcessors(new List<ITransactionStorage>()
            {
                analogRemotePointStorage, discreteRemotePointStorage,
                schemaEnergySourceStorage, schemaBreakerStorage,
                derGroupStorage, networkModelStorage });
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

        public List<AnalogRemotePointSummaryDTO> GetAllAnalogEntities()
        {
            return analogRemotePointSummaryJob.GetAllEntities();
        }

        public List<DiscreteRemotePointSummaryDTO> GetAllDiscreteEntities()
        {
            return discreteRemotePointSummaryJob.GetAllEntities();
        }
        
        public SubSchemaDTO GetSchema(long energySourceId)
        {
            return schemaRepresentation.GetSchema(energySourceId);
        }

        public SubSchemaConductingEquipmentEnergized GetEquipmentStates(long energySourceId)
        {
            return schemaRepresentation.GetEquipmentStates(energySourceId);
        }

        public List<EnergySourceDTO> GetSubstations()
        {
            return schemaRepresentation.GetSubstations();
        }

        public AnalogRemotePointSummaryDTO GetEntity(long globalId)
        {
            return analogRemotePointSummaryJob.GetEntity(globalId);
        }

        DiscreteRemotePointSummaryDTO IDiscreteRemotePointSummaryJob.GetEntity(long globalId)
        {
            return discreteRemotePointSummaryJob.GetEntity(globalId);
        }

        List<DERGroupSummaryDTO> IDERGroupSummaryJob.GetAllAnalogEntities()
        {
            return derGroupSummaryJob.GetAllEntities();
        }

        DERGroupSummaryDTO IDERGroupSummaryJob.GetEntity(long globalId)
        {
            return derGroupSummaryJob.GetEntity(globalId);
        }

        public SchemaEnergyBalanceDTO GetEnergyBalance(long energySourceGid)
        {
            return schemaRepresentation.GetEnergyBalance(energySourceGid);
        }

        public CommandFeedbackMessageDTO SendBreakerCommand(long breakerGid, int breakerValue)
        {
            return breakerCommanding.SendBreakerCommand(breakerGid, breakerValue);
        }

        public CommandFeedbackMessageDTO ValidateCommand(long breakerGid, int breakerValue)
        {
            return breakerCommanding.ValidateCommand(breakerGid, breakerValue);
        }

        public long SubStationContainsEntity(long entityGid)
        {
            return schemaRepresentation.SubStationContainsEntity(entityGid);
        }

        public CommandFeedbackMessageDTO SendCommand(long derGid, float commandingValue)
        {
            return derCommanding.SendCommand(derGid, commandingValue);
        }

        public CommandFeedbackMessageDTO ValidateCommand(long derGid, float commandingValue)
        {
            return derCommanding.ValidateCommand(derGid, commandingValue);
        }

        public List<NetworkModelEntityDTO> GetAllEntities()
        {
            return networkModelSummaryJob.GetAllEntities();
        }

        NetworkModelEntityDTO INetworkModelSummaryJob.GetEntity(long globalId)
        {
            return networkModelSummaryJob.GetEntity(globalId);
        }

        public ProductionForecastDTO GetProductionForecast(int hours)
        {
            return productionForecast.GetProductionForecast(hours);
        }

        public List<NetworkModelEntityDTO> GetAllEntities(List<DMSType> entityTypes)
        {
            return networkModelSummaryJob.GetAllEntities(entityTypes);
        }

        public SuggsetedCommandSequenceDTO GetSuggestedCommandSequence(CommandRequestDTO commandSequenceRequest, float setPoint)
        {
            return derOptimalCommandingProxy.GetSuggestedCommandSequence(commandSequenceRequest, setPoint);
        }

        public CommandFeedbackMessageDTO ExecuteCommandSequence(CommandSequenceRequest commandSequence)
        {
            return derOptimalCommandingProxy.ExecuteCommandSequence(commandSequence);
        }

        public List<WeatherDataInfo> GetHourlyWeatherForecast(int hours)
        {
            return weatherForecast.GetHourlyWeatherForecast(hours);
        }
    }
}
