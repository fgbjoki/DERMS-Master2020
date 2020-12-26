using System;
using System.ServiceModel;

namespace TransactionManager
{
    class Program
    {
        static void Main(string[] args)
        {
            TransactionManager ts = new TransactionManager();

            ServiceHost host = new ServiceHost(ts);
            host.Open();

            Console.WriteLine("Transaction service started...");
            Console.ReadLine();
        }
    }
}
