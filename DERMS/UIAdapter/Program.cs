using System;
using System.ServiceModel;

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
