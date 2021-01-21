using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace FieldProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            FieldProcessor fieldProcessor = new FieldProcessor();

            ServiceHost serviceHost = new ServiceHost(fieldProcessor);
            serviceHost.Open();

            Console.WriteLine("Field processor ready...");
            Console.ReadLine();
        }
    }
}
