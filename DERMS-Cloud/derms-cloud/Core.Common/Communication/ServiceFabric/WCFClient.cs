using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Client;
using System;

namespace Core.Common.Communication.ServiceFabric
{
    public class WCFClient<T> : ServicePartitionClient<WcfCommunicationClient<T>> 
        where T : class
    {

        internal WCFClient(ICommunicationClientFactory<WcfCommunicationClient<T>> communicationClientFactory,
                                                   Uri serviceUri,
                                                   ServicePartitionKey partitionKey = null,
                                                   TargetReplicaSelector targetReplicaSelector = TargetReplicaSelector.Default,
                                                   string listenerName = null,
                                                   OperationRetrySettings retrySettings = null)
            : base(communicationClientFactory, serviceUri, partitionKey, targetReplicaSelector, listenerName, retrySettings)
        {

        }
    }
}
