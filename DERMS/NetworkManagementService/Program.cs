using System;
using System.ServiceModel;

namespace NetworkManagementService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Netowrk Management Service";
            NetworkModel nms = new NetworkModel();

            ServiceHost host = new ServiceHost(nms);
            host.Open();

            Console.WriteLine("Service started...");
            Console.ReadLine();
        }
    }
}
