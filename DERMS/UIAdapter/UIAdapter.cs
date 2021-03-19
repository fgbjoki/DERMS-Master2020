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
using UIAdapter.DynamicHandlers;

namespace UIAdapter
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class UIAdapter : ITransaction, IModelPromotionParticipant, IAnalogRemotePointSummaryJob, IDiscreteRemotePointSummaryJob
    {
        private readonly string serviceName = "UIAdapter";
        private string serviceUrlForTransaction;

        private TransactionManager transactionManager;

        private AnalogRemotePointStorage analogRemotePointStorage;
        private DiscreteRemotePointStorage discreteRemotePointStorage;

        private AnalogRemotePointSummaryJob analogRemotePointSummaryJob;
        private DiscreteRemotePointSummaryJob discreteRemotePointSummaryJob;

        private DynamicHandlersManager dynamicHandlersManager;

        public UIAdapter()
        {
            LoadConfigurationFromAppConfig();

            transactionManager = new TransactionManager(serviceName, serviceUrlForTransaction);
            analogRemotePointStorage = new AnalogRemotePointStorage();
            discreteRemotePointStorage = new DiscreteRemotePointStorage();

            transactionManager.LoadTransactionProcessors(new List<ITransactionStorage>() { analogRemotePointStorage, discreteRemotePointStorage });

            InitializeJobs();
            InitializeDynamicHandlers();
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
            analogRemotePointSummaryJob = new AnalogRemotePointSummaryJob(analogRemotePointStorage);
            discreteRemotePointSummaryJob = new DiscreteRemotePointSummaryJob(discreteRemotePointStorage);
        }

        private void InitializeDynamicHandlers()
        {
            dynamicHandlersManager = new DynamicHandlersManager();
            dynamicHandlersManager.AddDynamicListeners(analogRemotePointStorage);
            dynamicHandlersManager.AddDynamicListeners(discreteRemotePointStorage);

            dynamicHandlersManager.StartListening();
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
    }
}
