using Common.Communication;
using Common.ServiceInterfaces;
using Common.ServiceInterfaces.Transaction;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.Threading.Tasks;

namespace NetworkManagementService.Components
{
    class TransactionStarter : ITransactionStarter
    {
        private List<WCFClient<IModelPromotionParticipant>> services;

        private WCFClient<ITransactionManager> transactionManager;

        private string serviceName;

        private string serviceEndpoint;

        public TransactionStarter(string serviceName, string serviceEndpoint)
        {
            this.serviceName = serviceName;
            this.serviceEndpoint = serviceEndpoint;

            transactionManager = new WCFClient<ITransactionManager>("transactionManagerEndpoint");

            InitializeServices();
        }

        public bool StartTransaction(IEnumerable<long> insertedGids)
        {
            try
            {
                if (!transactionManager.Proxy.StartEnlist())
                {
                    return false;
                }

                if (!transactionManager.Proxy.EnlistService(serviceName, serviceEndpoint))
                {
                    return false;
                }

                if (!InformOtherServicesForTransaction(services, insertedGids.ToList()))
                {
                    transactionManager.Proxy.EndEnlist(false);
                }

                if (!transactionManager.Proxy.EndEnlist(true))
                {
                    // THIS IS IS NOT SUPPOSED TO HAPPEN, IF IT DOES, TRANSCTION COORDINATOR IS IN FAULT
                    return false;
                }
            }
            catch (Exception e)
            {
                // log e
                return false;
            }

            return true;
        }

        private bool InformOtherServicesForTransaction(List<WCFClient<IModelPromotionParticipant>> clients, List<long> insertedGids)
        {
            bool areAllServicesReady = true;

            List<Task<bool>> sendingTasks = new List<Task<bool>>(clients.Count);

            foreach (var client in clients)
            {
                Task<bool> newTask = new Task<bool>(() => InformServiceForTransaction(client, insertedGids));
                newTask.Start();
                sendingTasks.Add(newTask);
            }

            Task.WaitAll(sendingTasks.ToArray());

            sendingTasks.ForEach(task => areAllServicesReady &= task.Result);

            return areAllServicesReady;
        }

        private bool InformServiceForTransaction(WCFClient<IModelPromotionParticipant> client, List<long> insertedGids)
        {
            try
            {
                return client.Proxy.ApplyChanges(insertedGids, new List<long>(0), new List<long>(0));
            }
            catch (Exception e)
            {
                // log e
                return false;
            }
        }

        private void InitializeServices()
        {
            NetTcpBinding binding = new NetTcpBinding();

            ClientSection clientSection = ConfigurationManager.GetSection("system.serviceModel/client") as ClientSection;
            services = new List<WCFClient<IModelPromotionParticipant>>(1);

            for (int i = 0; i < clientSection.Endpoints.Count; i++)
            {
                ChannelEndpointElement endpoint = clientSection.Endpoints[i];
                if (endpoint.Contract.Equals(typeof(IModelPromotionParticipant).ToString()))
                {
                    services.Add(new WCFClient<IModelPromotionParticipant>(endpoint.Name));
                }
            }
        }
    }
}
