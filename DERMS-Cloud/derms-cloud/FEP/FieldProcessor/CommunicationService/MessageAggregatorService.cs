using Core.Common.ListenerDepedencyInjection;
using Core.Common.ServiceInterfaces.FEP.MessageAggregator;
using MessageAggregatorService.MessageAggregator;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System.Collections.Generic;
using System.Fabric;
using System.Fabric.Description;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace MessageAggregatorService
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class MessageAggregatorService : StatefulService
    {
        private ObjectProxy<MessageValidator> messageValidator;
        private string localUrl = "net.tcp://host:port/MessageAggregator";
        public MessageAggregatorService(StatefulServiceContext context)
            : base(context)
        {
            messageValidator = new ObjectProxy<MessageValidator>();

            string host = context.NodeContext.IPAddressOrFQDN;
            EndpointResourceDescription endpoint = context.CodePackageActivationContext.GetEndpoint("ServiceEndpoint");
            int port = endpoint.Port;

            localUrl = localUrl.Replace("host", host).Replace("port", port.ToString());
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
                    string uri = localUrl + "/IResponseReceiver";
                    var listener = new WcfCommunicationListener<IResponseReceiver>(
                        wcfServiceObject: new CommandResponseReceiver(messageValidator, Log),
                        serviceContext: context,
                        listenerBinding: new NetTcpBinding(),
                        address: new EndpointAddress(uri)
                        );

                    return listener;
                }, "MessageAggregatorServiceIResponseReceiverService"),
                new ServiceReplicaListener((context) =>
                {
                    string uri = localUrl + "/ICommandSender";
                     var listener = new WcfCommunicationListener<ICommandSender>(
                        wcfServiceObject: new CommandSenderService(messageValidator, Log),
                        serviceContext: context,
                        listenerBinding: new NetTcpBinding(),
                        address: new EndpointAddress(uri)
                        );

                    return listener;
                }, "MessageAggregatorServiceICommandSenderService")
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
            messageValidator.Instance = new MessageValidator(StateManager, Log);
        }

        private void Log(string text)
        {
            ServiceEventSource.Current.ServiceMessage(Context, $"MessageAggregatorService - {text}");
        }
    }
}
