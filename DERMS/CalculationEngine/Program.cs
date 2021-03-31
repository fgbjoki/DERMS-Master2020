using System;
using System.ServiceModel;

namespace CalculationEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            CalculationEngine calculationEngine = new CalculationEngine();

            ServiceHost serviceHost = new ServiceHost(calculationEngine);
            serviceHost.Open();

            Console.WriteLine("Calculation Engine ready...");
            Console.ReadLine();
        }
    }
}
