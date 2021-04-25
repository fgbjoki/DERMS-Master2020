using Common.ComponentStorage.StorageItemCreator;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.GDA;
using CalculationEngine.TransactionProcessing.StorageTransactionProcessor.Topology;
using CalculationEngine.Model.Topology.Transaction;
using System;

namespace CalculationEngine.TransactionProcessing.StorageItemCreators.Topology
{
    class ConnectivityNodeStorageItemCreator : StorageItemCreator
    {
        private ReferenceResolver referenceResolver;

        public ConnectivityNodeStorageItemCreator(ReferenceResolver referenceResolver)
        {
            this.referenceResolver = referenceResolver;
        }

        public override IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            ConnectivityNode newConnectivityNode = new ConnectivityNode(rd.Id);

            referenceResolver.AddReference(newConnectivityNode);

            return newConnectivityNode;
        }

        public override Dictionary<DMSType, List<ModelCode>> GetNeededProperties()
        {
            return new Dictionary<DMSType, List<ModelCode>>()
            {
                { DMSType.CONNECTIVITYNODE, new List<ModelCode>() },
            };
        }
    }
}
