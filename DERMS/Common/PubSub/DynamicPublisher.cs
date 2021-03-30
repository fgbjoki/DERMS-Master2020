using NServiceBus;
using System.Threading.Tasks;

namespace Common.PubSub
{
    public class DynamicPublisher : IDynamicPublisher
    {
        private IEndpointInstance endpointInstance;

        public DynamicPublisher(string endpointName)
        {
            ConfigureEndpointInstance(endpointName);
        }

        public async Task Publish(IEvent message)
        {
            if (message == null)
            {
                return;
            }

            await endpointInstance.Publish(message);
        }

        public void Dispose()
        {
            endpointInstance.Stop().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private void ConfigureEndpointInstance(string endpointName)
        {
            var endpointConfiguration = new EndpointConfiguration(endpointName);
            var transport = endpointConfiguration.UseTransport<LearningTransport>();

            endpointInstance = Endpoint.Start(endpointConfiguration).ConfigureAwait(false).GetAwaiter().GetResult();
        }        
    }
}
