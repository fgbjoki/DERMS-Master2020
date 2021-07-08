using Core.Common.Communication.ServiceFabric.FEP;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using PollingService.Service;
using System.Collections.Generic;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;

namespace PollingService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class PollingService : StatelessService
    {
        private IPollingInvoker pollingIvoker;
        public PollingService(StatelessServiceContext context)
            : base(context)
        {
            pollingIvoker = new PollingInvoker(new FEPStorageWCFClient(), new CommandSenderWCFClient(), Log);
        }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[0];
        }

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            await pollingIvoker.StartAquisition(cancellationToken);
        }

        private void Log(string text)
        {
            ServiceEventSource.Current.ServiceMessage(Context, text);
        }
    }
}
