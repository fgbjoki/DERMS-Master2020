using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIAdapter
{
    class Program
    {
        static void Main()
        {
            TestMain().GetAwaiter().GetResult();
        }


        static async Task AsyncMain()
        {
            Console.Title = "UIAdapter";

            var endpointConfiguration = new EndpointConfiguration("UIAdapter");
            var transport = endpointConfiguration.UseTransport<LearningTransport>();
            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();

            await endpointInstance.Stop().ConfigureAwait(false);
        }

        static async Task TestMain()
        {
            Console.Title = "Test UIAdapter";

            var endpointConfiguration = new EndpointConfiguration("UIAdapter");
            RemotePointValueChangedHandler handler = new RemotePointValueChangedHandler("Pera");
            endpointConfiguration.RegisterComponents(
            registration: configureComponents =>
            {
                configureComponents.RegisterSingleton(handler);
            });
            endpointConfiguration.UseTransport<LearningTransport>();
            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();

            await endpointInstance.Stop().ConfigureAwait(false);
        }
    }
}
