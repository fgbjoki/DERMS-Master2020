using NServiceBus;

namespace Common.PubSub
{
    public class DynamicPublisher : IDynamicPublisher
    {
        private IEndpointInstance endpointInstance;

        public DynamicPublisher(string endpointName)
        {
            ConfigureEndpointInstance(endpointName);
        }

        public void Publish(IEvent message)
        {
            endpointInstance.Publish(message).ConfigureAwait(false);
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
