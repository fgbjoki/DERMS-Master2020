using NServiceBus;
using System;
using System.Collections.Generic;

namespace Common.PubSub
{
    public class DynamicHandlersManager : IDisposable
    {
        private string endpointName;

        private Dictionary<Type, object> handlers;
        private IEndpointInstance endpointInstance;

        public DynamicHandlersManager(string endpointName)
        {
            this.endpointName = endpointName;

            handlers = new Dictionary<Type, object>();
        }

        public void Dispose()
        {
            endpointInstance.Stop().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public void AddDynamicListeners(INServiceBusHandlerCreator storage)
        {
            foreach (object handler in storage.GetHandlers())
            {
                handlers.Add(handler.GetType(), handler);
            }
        }

        public async void StartListening()
        {
            var endpointConfiguration = new EndpointConfiguration(endpointName);
            //ConfigureEndpoint(endpointConfiguration);
            var transport = endpointConfiguration.UseTransport<LearningTransport>();
            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);
        }

        private void ConfigureEndpoint(EndpointConfiguration endpointConfiguration)
        {
            //endpointConfiguration.RegisterComponents(
            //registration: configureComponents =>
            //{
            //    foreach (var handler in handlers)
            //    {
            //        configureComponents.RegisterSingleton(handler.Key, handler.Value);
            //    }
            //});
        }
    }
}
