using CommandingService.CommandingProcessor;
using Core.Common.ServiceInterfaces.FEP.CommandingService;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System.Collections.Generic;
using System.Fabric;
using System.ServiceModel;

namespace CommandingService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class CommandingService : StatelessService
    {
        public CommandingService(StatelessServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new[] { new ServiceInstanceListener((context) =>
                {
                    var listener = new WcfCommunicationListener<ICommanding>(
                        wcfServiceObject: new ReceiveCommandingProcessor(Log),
                        serviceContext: context,
                        listenerBinding: new NetTcpBinding(SecurityMode.None),
                        endpointResourceName: "ServiceEndpointCE"
                    );
                    return listener;

                })
            };
        }

        private void Log(string text)
        {
            ServiceEventSource.Current.ServiceMessage(Context, text);
        }
    }
}
