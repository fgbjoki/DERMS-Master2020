using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace UIAdapter
{
    class Program
    {
        static void Main(string[] args)
        {
            UIAdapter uiAdapter = new UIAdapter();

            ServiceHost serviceHost = new ServiceHost(uiAdapter);
            serviceHost.Open();

            Console.WriteLine("UI Adapter started...");
            Console.ReadLine();
        }
    }
}
