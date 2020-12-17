using Common.ServiceInterfaces.Transaction;
using System.ServiceModel;

namespace TransactionManager
{
    /// <summary>
    /// Service resposible for distributed transaction
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    public class TransactionManager : ITransactionManager
    {
        private TransactionProcessor transactionProcessor;

        public TransactionManager()
        {
            transactionProcessor = new TransactionProcessor();
        }

        public bool EnlistService(string serviceName, string endpointAddress)
        {
            return transactionProcessor.EnlistService(serviceName, endpointAddress);
        }

        public bool StartEnlist()
        {
            return transactionProcessor.StartEnlist();
        }

        public bool EndEnlist(bool allServicesEnlisted)
        {
            return transactionProcessor.EndEnlist(allServicesEnlisted);
        }
    }
}
