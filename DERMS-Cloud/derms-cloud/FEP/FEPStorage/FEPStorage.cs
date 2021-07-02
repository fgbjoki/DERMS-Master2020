﻿using Core.Common.ListenerDepedencyInjection;
using Core.Common.ServiceInterfaces.NMS;
using Core.Common.ServiceInterfaces.Transaction;
using Core.Common.Transaction;
using Core.Common.Transaction.Storage;
using FEPStorage.Transaction.Storage;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Fabric.Description;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace FEPStorage
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class FEPStorage : StatefulService
    {
        private ObjectProxy<TransactionManager> transactionManager;
        private string localUrl = "net.tcp://host:port/FEPStorage";

        public FEPStorage(StatefulServiceContext context)
            : base(context)
        {
            string host = host = context.NodeContext.IPAddressOrFQDN;
            EndpointResourceDescription endpoint = context.CodePackageActivationContext.GetEndpoint("ServiceEndpoint");
            int port = endpoint.Port;

            localUrl = localUrl.Replace("host", host).Replace("port", port.ToString());

            transactionManager = new ObjectProxy<TransactionManager>();
        }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            var transactionParticipant = new TransactionParticipant<TransactionManager>(StateManager, Context, "FEPStorage", ServiceEventSource.Current.ServiceMessage, transactionManager);
            ServiceReplicaListener[] replicaListners = new ServiceReplicaListener[]
            {
                new ServiceReplicaListener((context) =>
                {
                    string uri = localUrl + "/Transaction";
                    var listener = new WcfCommunicationListener<ITransaction>(
                        wcfServiceObject: transactionParticipant,
                        serviceContext: context,
                        listenerBinding: new NetTcpBinding(),
                        address: new EndpointAddress(uri)
                        );

                    return listener;
                }, "FEPStorageTransactionService"),
                new ServiceReplicaListener((context) =>
                {
                    string uri = localUrl + "/IModelPromotionParticipant";
                     var listener = new WcfCommunicationListener<IModelPromotionParticipant>(
                        wcfServiceObject: transactionParticipant,
                        serviceContext: context,
                        listenerBinding: new NetTcpBinding(),
                        address: new EndpointAddress(uri)
                        );

                    return listener;
                }, "FEPStorageModelPromotionService")
            };

            return replicaListners;
        }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            InitializeTransactionManager();
        }

        private void InitializeTransactionManager()
        {
            Log("FEPStorage - Initializing storages for transaction");
            transactionManager.Instance = new TransactionManager(StateManager, "FEPStorage", localUrl + "/Transaction", Log);
            transactionManager.Instance.LoadTransactionProcessors(new List<ITransactionStorage>(2) { new AnalogRemotePointStorage(StateManager, Log), new DiscreteRemotePointStorage(StateManager, Log) });
            Log("FEPStorage - Initialization for transaction finished");
        }

        private void Log(string log)
        {
            ServiceEventSource.Current.ServiceMessage(Context, $"FEPStorage - {log}");
        }
    }
}