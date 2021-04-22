using Common.PubSub.Subscriptions;
using NServiceBus;
using System;
using System.Collections.Generic;

namespace Common.PubSub
{
    public class DynamicListenersManager : IDisposable
    {
        private string endpointName;

        private Dictionary<Topic, IDynamicListener> listeners;
        private IEndpointInstance endpointInstance;

        public DynamicListenersManager(string endpointName)
        {
            this.endpointName = endpointName;

            listeners = new Dictionary<Topic, IDynamicListener>();
        }

        public void Dispose()
        {
            endpointInstance.Stop().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public void AddDynamicHandlers(Topic topic, IDynamicListener handler)
        {
            listeners.Add(topic, handler);
        }

        public void ConfigureSubscriptions(IEnumerable<ISubscription> subscriptions)
        {
            foreach (var subscription in subscriptions)
            {
                IDynamicListener listener;
                if (!listeners.TryGetValue(subscription.Topic, out listener))
                {
                    Logger.Logger.Instance.Log($"[{this.GetType().Name}] Subscription with subscriber \'{subscription.Subscriber.GetType().Name}\' cannot find it's dynamic listener. Subscription rejected.");
                }

                listener.Subscribe(subscription);
            }
        }

        public IEndpointInstance StartListening(EndpointConfiguration endpointConfiguration)
        {
            ConfigureEndpoint(endpointConfiguration);
            var transport = endpointConfiguration.UseTransport<LearningTransport>();
            var endpointInstance = Endpoint.Start(endpointConfiguration).ConfigureAwait(false).GetAwaiter().GetResult();
            return endpointInstance;
        }

        private void ConfigureEndpoint(EndpointConfiguration endpointConfiguration)
        {
            endpointConfiguration.RegisterComponents(
            registration: configureComponents =>
            {
                foreach (var listener in listeners)
                {
                    configureComponents.RegisterSingleton(listener.Value.GetType(), listener.Value);
                }
            });
        }
    }
}
