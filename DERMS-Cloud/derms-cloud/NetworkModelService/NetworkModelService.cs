using Core.Common.ServiceInterfaces.NMS;
using Core.Common.ServiceInterfaces.Transaction;
using Core.Common.Transaction;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using NetworkManagementService;
using NetworkModelService.ServiceProviders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Fabric;
using System.Fabric.Description;
using System.ServiceModel;
using System.ServiceModel.Configuration;
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
                    string uri = $"net.tcp://{host}:{port}/NetworkModel/INetworkModelDeltaContract";
                    var listener = new WcfCommunicationListener<INetworkModelDeltaContract>(
                        wcfServiceObject: new DeltaServiceProvider(StateManager, this.Context, nmsInstance),
                        serviceContext: this.Context,
                        listenerBinding: new NetTcpBinding() {MaxBufferSize = 2147483647, MaxReceivedMessageSize = 2147483647},
                        address: new EndpointAddress(uri)
                        );

                    return listener;
                }),
                new ServiceReplicaListener((context) =>
                {
                    string host = host = context.NodeContext.IPAddressOrFQDN;

                    EndpointResourceDescription endpoint = context.CodePackageActivationContext.GetEndpoint("ServiceEndpoint");
                    int port = endpoint.Port;
                    string uri = LoadConfigurationFromAppConfig();
                    var listener = new WcfCommunicationListener<ITransaction>(
                        wcfServiceObject: new NMSTransactionParticipant(StateManager, this.Context, ServiceEventSource.Current.ServiceMessage),
                        serviceContext: this.Context,
                        listenerBinding: new NetTcpBinding(),
                        address: new EndpointAddress(uri)
                        );

                    return listener;
                })
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
            ServiceEventSource.Current.ServiceMessage(Context, "NMS - Initialization of NMS Service started");
            NetworkModel networkModel = new NetworkModel();
            var nmsInstanceReliableCollection = StateManager.GetOrAddAsync<IReliableDictionary<string, NetworkModel>>(nmsInstance).GetAwaiter().GetResult();

            using (var tx = StateManager.CreateTransaction())
            {
                if (!nmsInstanceReliableCollection.ContainsKeyAsync(tx, nmsInstance).GetAwaiter().GetResult())
                {
                    await nmsInstanceReliableCollection.AddAsync(tx, nmsInstance, networkModel);
                }

                await tx.CommitAsync();
            }

            var transactionInstanceReliableCollection = StateManager.GetOrAddAsync<IReliableDictionary<string, NetworkModel>>(TransactionParticipant.TransactionParticipantString).GetAwaiter().GetResult();
            using (var tx = StateManager.CreateTransaction())
            {
                if (!transactionInstanceReliableCollection.ContainsKeyAsync(tx, TransactionParticipant.TransactionParticipantString).GetAwaiter().GetResult())
                {
                    await transactionInstanceReliableCollection.AddAsync(tx, TransactionParticipant.TransactionParticipantString, networkModel);
                }

                await tx.CommitAsync();
            }

            ServiceEventSource.Current.ServiceMessage(Context, "NMS - Initialization of NMS Service finished");
            ServiceEventSource.Current.ServiceMessage(Context, "NMS - Started");
        }

        private string LoadConfigurationFromAppConfig()
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

            return serviceSection.Services[0].Host.BaseAddresses[0].BaseAddress + transactionAddition;
        }
    }
}
