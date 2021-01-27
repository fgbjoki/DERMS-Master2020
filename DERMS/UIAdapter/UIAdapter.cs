using Common.ComponentStorage;
using Common.ServiceInterfaces;
using Common.ServiceInterfaces.Transaction;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;
using UIAdapter.TransactionProcessing.Storages;

namespace UIAdapter
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class UIAdapter : ITransaction, IModelPromotionParticipant
    {
        private readonly string serviceName = "UIAdapter";
        private string serviceUrlForTransaction;

        private TransactionManager transactionManager;

        private RemotePointStorage remotePointStorage;

        public UIAdapter()
        {
            LoadConfigurationFromAppConfig();

            transactionManager = new TransactionManager(serviceName, serviceUrlForTransaction);
            remotePointStorage = new RemotePointStorage();

            transactionManager.LoadTransactionProcessors(new List<ITransactionStorage>() { remotePointStorage });
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
    }
}
