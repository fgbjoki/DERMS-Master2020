using CalculationEngine.Model.Topology;
using CalculationEngine.Model.Topology.Transaction;
using CalculationEngine.TransactionProcessing.StorageTransactionProcessor.Topology;
using Common.ComponentStorage;
using System.Collections.Generic;
using Common.ComponentStorage.StorageItemCreator;
using CalculationEngine.TransactionProcessing.StorageItemCreators.Topology;
using Common.AbstractModel;
using Common.Logger;
using CalculationEngine.Model.Topology.Graph;
using CalculationEngine.Graphs;
using CalculationEngine.Graphs.GraphProcessors;
using CalculationEngine.Model.Topology.Graph.Topology;
using CalculationEngine.Model.Topology.Graph.Schema;

namespace CalculationEngine.TransactionProcessing.Storage.Topology
{
    public class TopologyStorage : IStorage<ConnectivityObject>, ITransactionStorage
    {
        private BreakerMessageMapping breakerMessageMapping;
        private ReferenceResolver referenceResolver;
        private GraphsCreationProcessor graphCreationStorage;
        private ModelResourcesDesc modelResourceDesc;

        private IGraphProcessor<IMultipleRootGraph<TopologyGraphNode>> topologyAnalysis;
        private IGraphProcessor<ISingleRootGraph<SchemaGraphNode>> schema;

        public TopologyStorage(BreakerMessageMapping breakerMessageMapping, GraphBranchManipulator graphManipulator, IGraphProcessor<IMultipleRootGraph<TopologyGraphNode>> topologyAnalysis, IGraphProcessor<ISingleRootGraph<SchemaGraphNode>> schema)
        {
            this.breakerMessageMapping = breakerMessageMapping;
            this.topologyAnalysis = topologyAnalysis;
            this.schema = schema;

            modelResourceDesc = new ModelResourcesDesc();
            referenceResolver = new ReferenceResolver();

            // TODO ADD TOPOLOGY ANALYSIS AND SCHEMA
            graphCreationStorage = new GraphsCreationProcessor(modelResourceDesc, schema, topologyAnalysis);

            TerminalStorage = new TerminalStorage(referenceResolver);
            EnergySourceStorage = new EnergySourceStorage(referenceResolver);
            ConnectivityNodeStorage = new ConnectivityNodeStorage(referenceResolver);
            ConductingEquipment = new ConductingEquipmentStorage(referenceResolver, breakerMessageMapping);
        }

        protected TopologyStorage(BreakerMessageMapping breakerMessageMapping, ReferenceResolver referenceResolver, GraphsCreationProcessor graphCreationStorage)
        {
            this.referenceResolver = referenceResolver;
            this.breakerMessageMapping = breakerMessageMapping;
            this.graphCreationStorage = graphCreationStorage;

            TerminalStorage = new TerminalStorage(referenceResolver);
            EnergySourceStorage = new EnergySourceStorage(referenceResolver);
            ConnectivityNodeStorage = new ConnectivityNodeStorage(referenceResolver);
            ConductingEquipment = new ConductingEquipmentStorage(referenceResolver, breakerMessageMapping);
        }

        public IStorage<Terminal> TerminalStorage { get; private set; }
        public IStorage<EnergySource> EnergySourceStorage { get; private set; }
        public IStorage<ConductingEquipment> ConductingEquipment { get; private set; }
        public IStorage<ConnectivityNode> ConnectivityNodeStorage { get; private set; }

        public bool AddEntity(ConnectivityObject item)
        {
            DMSType dmsType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(item.GlobalId);

            switch (dmsType)
            {
                case DMSType.CONNECTIVITYNODE:
                    return ConnectivityNodeStorage.AddEntity(item as ConnectivityNode);
                case DMSType.TERMINAL:
                    return TerminalStorage.AddEntity(item as Terminal);
                case DMSType.ENERGYSOURCE:
                    return EnergySourceStorage.AddEntity(item as EnergySource);
                case DMSType.ENERGYSTORAGE:
                case DMSType.SOLARGENERATOR:
                case DMSType.WINDGENERATOR:
                case DMSType.ENERGYCONSUMER:
                case DMSType.ACLINESEG:
                case DMSType.BREAKER:
                    return ConductingEquipment.AddEntity(item as ConductingEquipment);
                default:
                    Logger.Instance.Log($"Storage \'TopologyStorage\' does not store dms type: {dmsType.ToString()}");
                    return false;
            }
        }

        public List<IStorageTransactionProcessor> GetStorageTransactionProcessors()
        {
            IStorageItemCreator conductingEquipmentStorageItemCreator = new ConductingEquipmentStorageItemCreator(referenceResolver);
            IStorageItemCreator derStorageItemCreator = new DERGeneratorStorageItemCreator(referenceResolver);

            Dictionary<DMSType, IStorageItemCreator> storageItemCreators = new Dictionary<DMSType, IStorageItemCreator>()
            {
                { DMSType.CONNECTIVITYNODE, new ConnectivityNodeStorageItemCreator(referenceResolver) },
                { DMSType.TERMINAL, new TerminalStorageItemCreator(referenceResolver) },
                { DMSType.ACLINESEG, conductingEquipmentStorageItemCreator },
                { DMSType.ENERGYCONSUMER, conductingEquipmentStorageItemCreator },
                { DMSType.ENERGYSTORAGE, new EnergyStorageStorageItemCreator(referenceResolver) },
                { DMSType.SOLARGENERATOR, derStorageItemCreator },
                { DMSType.WINDGENERATOR, derStorageItemCreator },
                { DMSType.BREAKER, new BreakerStorageItemCreator(breakerMessageMapping, referenceResolver) },
                { DMSType.ENERGYSOURCE, new EnergySourceStorageItemCreator(referenceResolver) }
            };

            return new List<IStorageTransactionProcessor>() { new GraphTransactionProcessor(this, storageItemCreators, graphCreationStorage) };
        }

        public bool EntityExists(long globalId)
        {
            DMSType dmsType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(globalId);

            switch (dmsType)
            {
                case DMSType.CONNECTIVITYNODE:
                    return ConnectivityNodeStorage.EntityExists(globalId);
                case DMSType.TERMINAL:
                    return TerminalStorage.EntityExists(globalId);
                case DMSType.ENERGYSOURCE:
                    return EnergySourceStorage.EntityExists(globalId);
                case DMSType.ENERGYSTORAGE:
                case DMSType.SOLARGENERATOR:
                case DMSType.WINDGENERATOR:
                case DMSType.ENERGYCONSUMER:
                case DMSType.ACLINESEG:
                case DMSType.BREAKER:
                    return ConductingEquipment.EntityExists(globalId);
                default:
                    Logger.Instance.Log($"Storage \'TopologyStorage\' does not store dms type: {dmsType.ToString()}");
                    return false;
            }
        }

        public List<ConnectivityObject> GetAllEntities()
        {
            // TODO FIX THIS
            return new List<ConnectivityObject>();
        }

        public ConnectivityObject GetEntity(long globalId)
        {
            DMSType dmsType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(globalId);

            switch (dmsType)
            {
                case DMSType.CONNECTIVITYNODE:
                    return ConnectivityNodeStorage.GetEntity(globalId);
                case DMSType.TERMINAL:
                    return TerminalStorage.GetEntity(globalId);
                case DMSType.ENERGYSOURCE:
                    return EnergySourceStorage.GetEntity(globalId);
                case DMSType.ENERGYSTORAGE:
                case DMSType.SOLARGENERATOR:
                case DMSType.WINDGENERATOR:
                case DMSType.ENERGYCONSUMER:
                case DMSType.ACLINESEG:
                case DMSType.BREAKER:
                    return ConductingEquipment.GetEntity(globalId);
                default:
                    Logger.Instance.Log($"Storage \'TopologyStorage\' does not store dms type: {dmsType.ToString()}");
                    return null;
            }
        }

        public bool ValidateEntity(ConnectivityObject entity)
        {
            return entity != null;
        }

        public object Clone()
        {
            return new TopologyStorage(breakerMessageMapping, referenceResolver, graphCreationStorage);
        }

        public void ShallowCopyEntities(IStorage<ConnectivityObject> storage)
        {
            TopologyStorage copyStorage = storage as TopologyStorage;

            TerminalStorage.ShallowCopyEntities(copyStorage.TerminalStorage);
            EnergySourceStorage.ShallowCopyEntities(copyStorage.EnergySourceStorage);
            ConductingEquipment.ShallowCopyEntities(copyStorage.ConductingEquipment);
            ConnectivityNodeStorage.ShallowCopyEntities(copyStorage.ConnectivityNodeStorage);
        }
    }
}
