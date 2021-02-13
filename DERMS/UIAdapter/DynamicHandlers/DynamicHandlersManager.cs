using NServiceBus;
using System;
using System.Collections.Generic;
using UIAdapter.TransactionProcessing.Storages;

namespace UIAdapter.DynamicHandlers
{
    public class DynamicHandlersManager : IDisposable
    {
        private Dictionary<Type, object> handlers;
        private IEndpointInstance endpointInstance;

        public DynamicHandlersManager()
        {
            handlers = new Dictionary<Type, object>();
        }

        public void Dispose()
        {
            endpointInstance.Stop().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public void AddDynamicListeners(INServiceBusStorage storage)
        {
            foreach (object handler in storage.GetHandlers())
            {
                handlers.Add(handler.GetType(), handler);
            }
        }

        public async void StartListening()
        {

            //ConfigureEndpoint(endpointConfiguration);

            var endpointConfiguration = new EndpointConfiguration("UIAdapter");
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
