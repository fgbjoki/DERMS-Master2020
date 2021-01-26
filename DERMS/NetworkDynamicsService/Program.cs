using Messages;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDynamicsService
{
    class Program
    {
        static ILog log = LogManager.GetLogger<Program>();


        static void Main()
        {
            AsyncMain().GetAwaiter().GetResult();
        }


        static async Task AsyncMain()
        {
            Console.Title = "NDS";

            var endpointConfiguration = new EndpointConfiguration("NDS");
            var transport = endpointConfiguration.UseTransport<LearningTransport>();
            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            await SendChangedValue(endpointInstance).ConfigureAwait(false);

            await endpointInstance.Stop().ConfigureAwait(false);
        }


        static async Task SendChangedValue(IEndpointInstance endpointInstance)
        {
            while(true)
            {
                log.Info($"Send new value ? (y - yes, q - QUIT) : ");
                var key = Console.ReadKey();
                Console.WriteLine();

                switch(key.Key)
                {
                    case ConsoleKey.Y:
                        Console.Write("GID: ");
                        var gid_d = long.Parse(Console.ReadLine());

                        Console.Write("New USHORT value: ");
                        var value_d = ushort.Parse(Console.ReadLine());

                        var remotePointValueChanged = new RemotePointValueChanged
                        {
                            GID = gid_d,
                            Value = value_d
                        };

                        log.Info($"Sending RemotePointValueChanged event, GID = {remotePointValueChanged.GID}");

                        await endpointInstance.Publish(remotePointValueChanged).ConfigureAwait(false);

                        break;

                    case ConsoleKey.Q:
                        return;

                    default:
                        log.Info("Unknown input. Please, try again.");
                        break;
                }
            }
        }
    }
}
