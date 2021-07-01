using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Wcf;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Client;
using System;
using System.ServiceModel.Channels;

namespace Core.Common.Communication.ServiceFabric
{
    public abstract class BaseServiceFabricWCFClient<T>
        where T : class
    {
        private readonly string serviceUri;

        protected BaseServiceFabricWCFClient(string serviceUri)
        {
            this.serviceUri = serviceUri;
        }

        protected WCFClient<T> BuildClient()
        {
            Binding binding = WcfUtility.CreateTcpClientBinding();
            IServicePartitionResolver partitionResolver = ServicePartitionResolver.GetDefault();
            var wcfClientFactory = new WcfCommunicationClientFactory<T>(clientBinding: binding, servicePartitionResolver: partitionResolver);
            var ServiceUri = new Uri(serviceUri);
            var client = new WCFClient<T>(wcfClientFactory, ServiceUri);
            return client;
        }

        protected WCFClient<T> BuildClient(long partitionKey)
        {
            Binding binding = WcfUtility.CreateTcpClientBinding();
            IServicePartitionResolver partitionResolver = ServicePartitionResolver.GetDefault();
            var wcfClientFactory = new WcfCommunicationClientFactory<T>(clientBinding: binding, servicePartitionResolver: partitionResolver);
            var ServiceUri = new Uri(serviceUri);
            var client = new WCFClient<T>(wcfClientFactory, ServiceUri, new ServicePartitionKey(partitionKey));
            return client;
        }

    }
}
