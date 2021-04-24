using System;
using System.ServiceModel;

namespace FieldProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Field Processor Service";
            FieldProcessor fieldProcessor = new FieldProcessor();

            ServiceHost serviceHost = new ServiceHost(fieldProcessor);
            serviceHost.Open();

            Console.WriteLine("Field processor ready...");
            Console.ReadLine();
        }
    }
}
