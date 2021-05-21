using Common.ServiceInterfaces;
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
            var binding = new NetTcpBinding();
            binding.MaxBufferSize = 2147483647;
            binding.MaxReceivedMessageSize = 2147483647;
            host.AddServiceEndpoint(typeof(INetworkModelDeltaContract), binding, "net.tcp://localhost:11112/NetworkModel/INetworkModelDeltaContract");
            host.Open();

            Console.WriteLine("Service started...");
            Console.ReadLine();
        }
    }
}
