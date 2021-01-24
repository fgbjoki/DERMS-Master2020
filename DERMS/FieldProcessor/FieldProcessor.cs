using Common.ComponentStorage;
using Common.SCADA.FieldProcessor;
using Common.ServiceInterfaces;
using Common.ServiceInterfaces.Transaction;
using Common.ServiceLocator;
using FieldProcessor.CommandingProcessor;
using FieldProcessor.MessageValidation;
using FieldProcessor.PollingRequestCreator;
using FieldProcessor.RemotePointAddressCollector;
using FieldProcessor.SimulatorState;
using FieldProcessor.TCPCommunicationHandler;
using FieldProcessor.TransactionProcessing.Storages;
using FieldProcessor.ValueExtractor;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.Threading;

namespace FieldProcessor
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class FieldProcessor : ITransaction, IModelPromotionParticipant, ICommanding 
    {
        private readonly string serviceName = "FieldProcessor";
        private string serviceUrlForTransaction;

        private FieldCommunicationHandler fieldCommunicationHandler;

        private BlockingQueue<byte[]> requestQueue;
        private BlockingQueue<byte[]> responseQueue;

        private ModbusMessageArbitrator messageArbitrator;

        private PollingInvoker pollingInvoker;

        private ICommandSender commandSender;

        private IPointValueExtractor pointValueExtractor;

        private IRemotePointSortedAddressCollector remotePointSortedAddressCollector;
        private IRemotePointRangeAddressCollector remotePointRangeAddressCollector;

        private ICommanding commandingProcessor;

        private SimulatorStateNotifier simulatorState;

        private TransactionManager transactionManager;

        private DiscreteRemotePointStorage discreteStorage;
        private AnalogRemotePointStorage analogStorage;

        public FieldProcessor()
        {
            InitializeQueues();

            messageArbitrator = new ModbusMessageArbitrator(responseQueue);

            InitializeRemotePointAddressCollectors();

            InitializeValueExtractor();

            simulatorState = new SimulatorStateNotifier();
            
            commandSender = new MessageValidator(responseQueue, requestQueue, pointValueExtractor);

            pollingInvoker = new PollingInvoker(remotePointRangeAddressCollector, commandSender, simulatorState);

            commandingProcessor = new ReceiveCommandingProcessor(commandSender, discreteStorage, analogStorage);

            InitializeFieldCommunicationHandler();

            LoadConfigurationFromAppConfig();
            InitializeForTransaction();
        }

        private void InitializeFieldCommunicationHandler()
        {
            AutoResetEvent sendDone = new AutoResetEvent(false);
            AsynchronousTCPClient tcpClient = new AsynchronousTCPClient("127.0.0.1", 22222, sendDone, messageArbitrator, simulatorState);
            fieldCommunicationHandler = new FieldCommunicationHandler(requestQueue, tcpClient, sendDone);
        }

        private void InitializeForTransaction()
        {
            discreteStorage = new DiscreteRemotePointStorage();
            analogStorage = new AnalogRemotePointStorage();
            transactionManager = new TransactionManager(serviceName, serviceUrlForTransaction);
            transactionManager.LoadTransactionProcessors(new List<ITransactionStorage>() { discreteStorage, analogStorage });
        }

        private void InitializeRemotePointAddressCollectors()
        {
            remotePointRangeAddressCollector = new RemotePointRangeAddressCollector();
            remotePointSortedAddressCollector = new RemotePointSortedAddressCollector();
            ServiceLocator.AddService(remotePointSortedAddressCollector);
            ServiceLocator.AddService(remotePointRangeAddressCollector);
        }

        private void LoadConfigurationFromAppConfig()
        {
            ServicesSection serviceSection = ConfigurationManager.GetSection("system.serviceModel/services") as ServicesSection;
            ServiceEndpointElementCollection endpoints = serviceSection.Services[0].Endpoints;
            string transactionAddition = String.Empty;
            for (int i = 0; i < endpoints.Count; i++)
            {
                ServiceEndpointElement endpoint = endpoints[i];
                if (endpoint.Contract.Equals(typeof(ITransaction).ToString()))
                {
                    transactionAddition = $"/{endpoint.Address.OriginalString}";
                }
            }

            serviceUrlForTransaction = serviceSection.Services[0].Host.BaseAddresses[0].BaseAddress + transactionAddition;
        }

        private void InitializeValueExtractor()
        {
            pointValueExtractor = new PointValueExtractor(remotePointSortedAddressCollector);
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
