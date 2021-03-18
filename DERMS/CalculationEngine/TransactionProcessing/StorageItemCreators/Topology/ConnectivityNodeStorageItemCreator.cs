using Common.ComponentStorage.StorageItemCreator;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.GDA;
using CalculationEngine.TransactionProcessing.StorageTransactionProcessor.Topology;
using CalculationEngine.Model.Topology.Transaction;

namespace CalculationEngine.TransactionProcessing.StorageItemCreators.Topology
{
    class ConnectivityNodeStorageItemCreator : StorageItemCreator
    {
        private ReferenceResolver referenceResolver;

        public ConnectivityNodeStorageItemCreator(ReferenceResolver referenceResolver) : base(CreatePropertiesPerType())
        {
            this.referenceResolver = referenceResolver;
        }

        public override IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            ConnectivityNode newConnectivityNode = new ConnectivityNode(rd.Id);

            referenceResolver.AddReference(newConnectivityNode);

            return newConnectivityNode;
        }

        private static Dictionary<DMSType, List<ModelCode>> CreatePropertiesPerType()
        {
            return new Dictionary<DMSType, List<ModelCode>>()
            {
                { DMSType.CONNECTIVITYNODE, new List<ModelCode>() },
            };
        }
    }
}
