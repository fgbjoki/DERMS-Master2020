using Common.ComponentStorage.StorageItemCreator;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.GDA;
using Common.Logger;
using CalculationEngine.Model.Topology.Transaction;
using CalculationEngine.TransactionProcessing.StorageTransactionProcessor.Topology;
using System;

namespace CalculationEngine.TransactionProcessing.StorageItemCreators.Topology
{
    public class TerminalStorageItemCreator : StorageItemCreator
    {
        private ReferenceResolver referenceResolver;

        public TerminalStorageItemCreator(ReferenceResolver referenceResolver) : base()
        {
            this.referenceResolver = referenceResolver;
        }

        public override IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            long connectivityNodeGid = rd.GetProperty(ModelCode.TERMINAL_CONNECTIVITYNODE).AsReference();

            ConnectivityNode connectivityNode = referenceResolver.GetConnectivityNode(connectivityNodeGid);
            if (connectivityNode == null)
            {
                Logger.Instance.Log($"[{this.GetType().Name}] Couldn't find connectivity node with gid: {connectivityNodeGid:X16}. Skipping terminal with gid {rd.Id:X16}!");
                return null;
            }

            Terminal newTerminal = new Terminal(rd.Id);
            connectivityNode.AddTerminal(newTerminal);

            referenceResolver.AddReference(newTerminal);

            return newTerminal;
        }

        public override Dictionary<DMSType, List<ModelCode>> GetNeededProperties()
        {
            return new Dictionary<DMSType, List<ModelCode>>()
            {
                { DMSType.TERMINAL, new List<ModelCode>() { ModelCode.TERMINAL_CONNECTIVITYNODE } },
            };
        }
    }
}
