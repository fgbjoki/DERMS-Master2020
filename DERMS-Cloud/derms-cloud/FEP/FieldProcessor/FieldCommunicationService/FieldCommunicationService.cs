using Core.Common.Communication.ServiceFabric.FEP;
using Core.Common.ServiceInterfaces.FEP.FieldCommunicator;
using FieldProcessor.TCPCommunicationHandler;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System.Collections.Generic;
using System.Fabric;
using System.Fabric.Description;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace FieldCommunicationService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class FieldCommunicationService : StatelessService
    {
        private string localUrl = "net.tcp://host:port/FieldCommunicationService";
        private AsynchronousTCPClient communication;

        public FieldCommunicationService(StatelessServiceContext context)
            : base(context)
        {
            communication = new AsynchronousTCPClient("127.0.0.1", 22222, new ResponseReceiverWCFClient(), Log);

            string host = context.NodeContext.IPAddressOrFQDN;
            EndpointResourceDescription endpoint = context.CodePackageActivationContext.GetEndpoint("ServiceEndpoint");
            int port = endpoint.Port;

            localUrl = localUrl.Replace("host", host).Replace("port", port.ToString());
        }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new[] { new ServiceInstanceListener((context) =>
                {
                    var listener = new WcfCommunicationListener<IFiledCommunicator>(
                        wcfServiceObject: communication,
                        serviceContext: context,
                        listenerBinding: new NetTcpBinding(),
                        address: new EndpointAddress(localUrl + "/IFiledCommunicator")
                    );
                    return listener;
                })
            };
        }

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                communication.Initialize(cancellationToken);
            });
        }

        private void Log(string text)
        {
            ServiceEventSource.Current.ServiceMessage(Context, $"{GetType().Name} - {text}");
        }
    }
}
