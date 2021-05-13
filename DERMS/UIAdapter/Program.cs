using System;
using System.ServiceModel;

namespace UIAdapter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "UI Adapter Service";
            UIAdapter uiAdapter = new UIAdapter();

            ServiceHost serviceHost = new ServiceHost(uiAdapter);
            serviceHost.Open();

            Console.WriteLine("UI Adapter started...");
            Console.ReadLine();
        }
    }
}
