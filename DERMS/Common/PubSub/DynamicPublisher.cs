using NServiceBus;
using System.Threading.Tasks;

namespace Common.PubSub
{
    public class DynamicPublisher : IDynamicPublisher
    {
        public DynamicPublisher()
        {

        }

        public async Task Publish(IEvent message)
        {
            if (message == null)
            {
                return;
            }

            await EndpointInstance.Publish(message);
        }

        public IEndpointInstance EndpointInstance { get; set; }

        public EndpointConfiguration ConfigureEndpointInstance(string endpointName)
        {
            var endpointConfiguration = new EndpointConfiguration(endpointName);

            return endpointConfiguration;
        }        
    }
}
