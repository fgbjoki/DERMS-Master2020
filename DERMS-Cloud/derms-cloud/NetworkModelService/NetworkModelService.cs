using Core.Common.ServiceInterfaces.NMS;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using NetworkManagementService;
using NetworkModelService.ServiceProviders;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Fabric.Description;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkModelService
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class NetworkModelService : StatefulService
    {
        private readonly string nmsInstance = "nmsInstance";

        public NetworkModelService(StatefulServiceContext context)
            : base(context)
        {
            NetworkModel networkModel = new NetworkModel();
            var tempInstance = StateManager.GetOrAddAsync<IReliableDictionary<string, INetworkModelDeltaContract>>(nmsInstance).GetAwaiter().GetResult();
            
            using (var tx = StateManager.CreateTransaction())
            {
                if (!tempInstance.ContainsKeyAsync(tx, nmsInstance).GetAwaiter().GetResult())
                {
                    tempInstance.AddAsync(tx, nmsInstance, networkModel);
                }
            }
        }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            ServiceReplicaListener[] replicaListners = new ServiceReplicaListener[]
            {
                new ServiceReplicaListener((context) =>
                {
                    string host = host = context.NodeContext.IPAddressOrFQDN;

                    EndpointResourceDescription endpoint = context.CodePackageActivationContext.GetEndpoint("ServiceEndpoint");
                    int port = endpoint.Port;
                    string uri = $"net.tcp://{host}:{port}/NetworkServiceModel";
                    var listener = new WcfCommunicationListener<INetworkModelDeltaContract>(
                        wcfServiceObject: new DeltaServiceProvider(StateManager, this.Context, "nmsInstance"),
                        serviceContext: this.Context,
                        listenerBinding: new NetTcpBinding() {MaxBufferSize = 2147483647, MaxReceivedMessageSize = 2147483647},
                        address: new EndpointAddress(uri)
                        );

                    return listener;
                }),
            };

            return replicaListners;
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            ServiceEventSource.Current.ServiceMessage(Context, "NMS - Started");
        }
    }
}
