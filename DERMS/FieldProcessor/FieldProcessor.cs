using Common.ComponentStorage;
using Common.SCADA.FieldProcessor;
using Common.ServiceInterfaces;
using Common.ServiceInterfaces.Transaction;
using Common.ServiceLocator;
using FieldProcessor.CommandingProcessor;
using FieldProcessor.MessageValidation;
using FieldProcessor.TCPCommunicationHandler;
using FieldProcessor.TransactionProcessing.Storages;
using FieldProcessor.ValueExtractor;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;

namespace FieldProcessor
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class FieldProcessor : ITransaction, IModelPromotionParticipant, ICommanding 
    {
        private FieldCommunicationHandler fieldCommunicationHandler;

        private BlockingQueue<byte[]> requestQueue;
        private BlockingQueue<byte[]> responseQueue;

        private ModbusMessageArbitrator messageArbitrator;

        private ICommandSender commandSender;

        private IPointValueExtractor pointValueExtractor;

        private IRemotePointAddressCollector remotePointAddressCollector;

        private ICommanding commandingProcessor;

        private TransactionManager transactionManager;

        private DiscreteRemotePointStorage discreteStorage;
        private AnalogRemotePointStorage analogStorage;

        public FieldProcessor()
        {
            InitializeQueues();

            messageArbitrator = new ModbusMessageArbitrator(responseQueue);

            InitializeValueExtractor();

            commandSender = new MessageValidator(responseQueue, requestQueue, pointValueExtractor);

            InitializeForTransaction();

            commandingProcessor = new ReceiveCommandingProcessor(commandSender, discreteStorage, analogStorage);

            InitializeFieldCommunicationHandler();
            InitializeForTransaction();
        }

        private void InitializeFieldCommunicationHandler()
        {
            AutoResetEvent sendDone = new AutoResetEvent(false);
            AsynchronousTCPClient tcpClient = new AsynchronousTCPClient("127.0.0.1", 22222, sendDone, messageArbitrator);
            fieldCommunicationHandler = new FieldCommunicationHandler(requestQueue, tcpClient, sendDone);
        }

        private void InitializeForTransaction()
        {
            discreteStorage = new DiscreteRemotePointStorage();
            analogStorage = new AnalogRemotePointStorage();
            transactionManager = new TransactionManager("SCADA", "TODO");
            transactionManager.LoadTransactionProcessors(new List<ITransactionStorage>() { discreteStorage, analogStorage });
        }

        private void InitializeValueExtractor()
        {
            remotePointAddressCollector = new RemotePointAddressCollector();
            ServiceLocator.AddService(remotePointAddressCollector);

            pointValueExtractor = new PointValueExtractor(remotePointAddressCollector);
        }

        private void InitializeQueues()
        {
            requestQueue = new BlockingQueue<byte[]>();
            responseQueue = new BlockingQueue<byte[]>();
        }

        public bool Prepare()
        {
            return transactionManager.Prepare();
        }

        public bool Commit()
        {
            return transactionManager.Commit();
        }

        public bool Rollback()
        {
            return transactionManager.Rollback();
        }

        public bool ApplyChanges(List<long> insertedEntities, List<long> updatedEntities, List<long> deletedEntities)
        {
            return transactionManager.ApplyChanges(insertedEntities, updatedEntities, deletedEntities);
        }

        public bool SendCommand(Command command)
        {
            return commandingProcessor.SendCommand(command);
        }
    }
}
