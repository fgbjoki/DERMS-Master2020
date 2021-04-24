using System;
using System.ServiceModel;

namespace NetworkDynamicsService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Network Dynamics Service";
            NetworkDynamicsService networkDynamicsService = new NetworkDynamicsService();

            ServiceHost serviceHost = new ServiceHost(networkDynamicsService);
            serviceHost.Open();

            Console.WriteLine("Network dynamics service ready...");
            Console.ReadLine();
        }
    }
}
