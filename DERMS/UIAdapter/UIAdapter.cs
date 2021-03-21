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
using UIAdapter.PubSub.DynamicHandlers;

namespace UIAdapter
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class UIAdapter : ITransaction, IModelPromotionParticipant, IAnalogRemotePointSummaryJob
    {
        private readonly string serviceName = "UIAdapter";
        private string serviceUrlForTransaction;

        private TransactionManager transactionManager;

        private AnalogRemotePointStorage analogRemotePointStorage;
        private DiscreteRemotePointStorage discreteRemotePointStorage;

        private AnalogRemotePointSummaryJob analogRemotePointSummaryJob;

        private DynamicListenersManager dynamicListenerManager;

        public UIAdapter()
        {
            LoadConfigurationFromAppConfig();

            transactionManager = new TransactionManager(serviceName, serviceUrlForTransaction);

            InitializeTransactionStorages();

            InitializeJobs();

            InitializePubSub();
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

        public List<AnalogRemotePointSummaryDTO> GetAllEntities()
        {
            return analogRemotePointSummaryJob.GetAllEntities();
        }

        private void InitializeJobs()
        {
            analogRemotePointSummaryJob = new AnalogRemotePointSummaryJob(analogRemotePointStorage);
        }

        private void InitializeDynamicHandlers()
        {
            dynamicListenerManager.ConfigureSubscriptions(analogRemotePointStorage.GetSubscriptions());
        }

        private void InitializeDynamicListeners()
        {
            dynamicListenerManager = new DynamicListenersManager("UIAdapter");
            List<IDynamicListener> listeners = new List<IDynamicListener>()
            {
                new AnalogRemotePointChangedListener()
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

            dynamicListenerManager.StartListening();
        }

        private void InitializeTransactionStorages()
        {
            analogRemotePointStorage = new AnalogRemotePointStorage();
            discreteRemotePointStorage = new DiscreteRemotePointStorage();

            transactionManager.LoadTransactionProcessors(new List<ITransactionStorage>() { analogRemotePointStorage, discreteRemotePointStorage });
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
    }
}
