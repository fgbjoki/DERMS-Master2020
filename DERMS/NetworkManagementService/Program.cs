using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace NetworkManagementService
{
    class Program
    {
        static void Main(string[] args)
        {
            NetworkModel nms = new NetworkModel();

            ServiceHost host = new ServiceHost(nms);
            host.Open();

            Console.WriteLine("Service started...");
            Console.ReadLine();
        }
    }
}
