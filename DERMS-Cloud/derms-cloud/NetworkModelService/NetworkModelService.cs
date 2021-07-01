using Common.ServiceInterfaces;
using Core.Common.GDA;
using Core.Common.ListenerDepedencyInjection;
using Core.Common.ServiceInterfaces.NMS;
using Core.Common.ServiceInterfaces.Transaction;
using Core.Common.Transaction;
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
        private ObjectProxy<NetworkModel> networkModel;

        public NetworkModelService(StatefulServiceContext context)
            : base(context)
        {
            networkModel = new ObjectProxy<NetworkModel>();
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
                    string host = context.NodeContext.IPAddressOrFQDN;

                    EndpointResourceDescription endpoint = context.CodePackageActivationContext.GetEndpoint("ServiceEndpoint");
                    int port = endpoint.Port;
                    string uri = $"net.tcp://{host}:{port}/NetworkModel/INetworkModelDeltaContract";
                    var listener = new WcfCommunicationListener<INetworkModelDeltaContract>(
                        wcfServiceObject: new DeltaServiceProvider(this.Context, networkModel),
                        serviceContext: context,
                        listenerBinding: new NetTcpBinding() {MaxBufferSize = 2147483647, MaxReceivedMessageSize = 2147483647},
                        address: new EndpointAddress(uri)
                        );

                    return listener;
                }, "NMSDeltaService"),
                new ServiceReplicaListener((context) =>
                {
                    string host = context.NodeContext.IPAddressOrFQDN;

                    EndpointResourceDescription endpoint = context.CodePackageActivationContext.GetEndpoint("ServiceEndpoint");
                    int port = endpoint.Port;
                    string uri = $"net.tcp://{host}:{port}/NetworkModel/INetworkModelGDAContract";
                    var listener = new WcfCommunicationListener<INetworkModelGDAContract>(
                        wcfServiceObject: new GDAServiceProvider(this.Context, networkModel),
                        serviceContext: context,
                        listenerBinding: new NetTcpBinding(),
                        address: new EndpointAddress(uri)
                        );

                    return listener;
                }, "NMSGDAService"),
                new ServiceReplicaListener((context) =>
                {
                    string host = host = context.NodeContext.IPAddressOrFQDN;
                    string uri = LoadConfigurationFromAppConfig(host);
                    var listener = new WcfCommunicationListener<ITransaction>(
                        wcfServiceObject: new TransactionParticipant<NetworkModel>(StateManager, Context, "NMS", ServiceEventSource.Current.ServiceMessage, networkModel),
                        serviceContext: context,
                        listenerBinding: new NetTcpBinding(),
                        address: new EndpointAddress(uri)
                        );

                    return listener;
                }, "NMSTransactionService")
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
            networkModel.Instance = new NetworkModel(StateManager);

            ServiceEventSource.Current.ServiceMessage(Context, "NMS - Initialization of NMS Service finished");
            ServiceEventSource.Current.ServiceMessage(Context, "NMS - Started");
        }

        private string LoadConfigurationFromAppConfig(string host)
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

            return (serviceSection.Services[0].Host.BaseAddresses[0].BaseAddress + transactionAddition).Replace("localhost", host);
        }
    }
}
