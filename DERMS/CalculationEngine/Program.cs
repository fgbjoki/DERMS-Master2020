using System;
using System.ServiceModel;

namespace CalculationEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Calculation Engine Service";
            CalculationEngine calculationEngine = new CalculationEngine();

            ServiceHost serviceHost = new ServiceHost(calculationEngine);
            serviceHost.Open();

            Console.WriteLine("Calculation Engine ready...");
            Console.WriteLine("Press enter to debug...");
            Console.ReadLine();
            calculationEngine.Compute();
            Console.WriteLine("Press enter to stop the service...");
            Console.ReadLine();
        }
    }
}
