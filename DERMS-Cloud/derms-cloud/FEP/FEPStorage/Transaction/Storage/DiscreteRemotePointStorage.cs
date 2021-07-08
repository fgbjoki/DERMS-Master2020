using Core.Common.AbstractModel;
using Core.Common.ReliableCollectionProxy;
using Core.Common.Transaction.Models.FEP.FEPStorage;
using Core.Common.Transaction.Storage;
using Core.Common.Transaction.StorageItemCreator;
using Core.Common.Transaction.StorageTransactionProcessor;
using FEPStorage.Transaction.StorageTransactionProcessor;
using FieldProcessor.TransactionProcessing.StorageItemCreators;
using Microsoft.ServiceFabric.Data;
using System;
using System.Collections.Generic;

namespace FEPStorage.Transaction.Storage
{
    public class DiscreteRemotePointStorage : Storage<DiscreteRemotePoint>
    {
        private Action<string> logAction;

        private string usedAddressesVariable;

        public DiscreteRemotePointStorage(IReliableStateManager stateManager, Action<string> logAction) : base(stateManager, "Discrete remote point storage")
        {
            this.logAction = logAction;
            usedAddressesVariable = "usedAddresses" + storageName;
            ReliableVariableProxy.SetVariable(stateManager, new HashSet<ushort>(), usedAddressesVariable);
        }

        protected DiscreteRemotePointStorage(IReliableStateManager stateManager, string storageName, string dictionaryName) : base(stateManager, storageName, dictionaryName)
        {
            usedAddressesVariable = "usedAddresses" + storageName;
            ReliableVariableProxy.SetVariable(stateManager, new HashSet<ushort>(), usedAddressesVariable);
        }

        public override List<IStorageTransactionProcessor> GetStorageTransactionProcessors()
        {
            var storageItemCreators = new Dictionary<DMSType, IStorageItemCreator>()
            {
                { DMSType.MEASUREMENTDISCRETE, new DiscreteStorageItemCreator() }
            };

            //RemotePointSortedAddressCollector remotePointAddressCollector = ServiceLocator.GetService<RemotePointSortedAddressCollector>();
            //RemotePointRangeAddressCollector rangeAddressCollector = ServiceLocator.GetService<RemotePointRangeAddressCollector>();

            return new List<IStorageTransactionProcessor>() { new DiscreteTransactionProcessor(this, storageItemCreators, logAction) };
        }

        public override bool AddEntity(DiscreteRemotePoint entity)
        {
            if (entity == null)
            {
                Log($"[{storageName}] Cannot add null to storage!");
                return false;
            }

            HashSet<ushort> addresses = ReliableVariableProxy.GetVariable<HashSet<ushort>>(StateManager, usedAddressesVariable);
            if (addresses == null)
            {
                addresses = new HashSet<ushort>();
                ReliableVariableProxy.SetVariable(stateManager, addresses, usedAddressesVariable);
            }

            if (addresses.Contains(entity.Address))
            {
                Log($"[{storageName}] Remote point with address : {entity.Address} already exists!");
                return false;
            }

            addresses.Add(entity.Address);
            ReliableVariableProxy.SetVariable(stateManager, addresses, usedAddressesVariable);

            bool addSucceded = base.AddEntity(entity);

            if (!addSucceded)
            {
                addresses.Remove(entity.Address);
                ReliableVariableProxy.SetVariable(stateManager, addresses, usedAddressesVariable);
            }

            return addSucceded;
        }

        protected override IStorage<DiscreteRemotePoint> CreateNewStorage(string name, string dictionaryName)
        {
            return new DiscreteRemotePointStorage(StateManager, name, dictionaryName)
            {
                logAction = logAction,
                usedAddressesVariable = usedAddressesVariable + "cloned"
            };
        }

        protected override void Log(string text)
        {
            logAction(text);
        }
    }
}
