using CalculationEngine.Model.Topology.Transaction;
using Common.ComponentStorage;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage.StorageItemCreator;
using Common.GDA;
using System.Linq;
using CalculationEngine.TransactionProcessing.Storage.Topology;
using System.Diagnostics;
using CalculationEngine.Graphs;

namespace CalculationEngine.TransactionProcessing.StorageTransactionProcessor.Topology
{
    public class GraphTransactionProcessor : StorageTransactionProcessor<ConnectivityObject>
    {
        private TopologyOrderProcessingEnumerable orderEnumerable;
        private GraphsCreationProcessor graphCreationStorage;

        public GraphTransactionProcessor(IStorage<ConnectivityObject> storage, Dictionary<DMSType, IStorageItemCreator> storageItemCreators, GraphsCreationProcessor graphCreationStorage) : base(storage, storageItemCreators)
        {
            this.graphCreationStorage = graphCreationStorage;

            orderEnumerable = new TopologyOrderProcessingEnumerable(modelRescDesc);
        }
        
        public override bool Commit()
        {
            return base.Commit() & graphCreationStorage.Commit();
        }

        public override bool Rollback()
        {
            return base.Rollback() & graphCreationStorage.Rollback();
        }

        protected override IEnumerable<KeyValuePair<DMSType, List<ResourceDescription>>> GetProcessingOrder(Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            orderEnumerable.PrepareEnumerator(affectedEntities);
            return orderEnumerable;
        }

        protected override List<DMSType> GetPrimaryTypes()
        {
            List<DMSType> primaryDMSTypes = modelRescDesc.GetLeaves(ModelCode.CONDUCTINGEQ);
            primaryDMSTypes.Add(DMSType.CONNECTIVITYNODE);
            primaryDMSTypes.Add(DMSType.TERMINAL);

            return primaryDMSTypes;
        }

        protected override void AddAdditionalEntities(Dictionary<DMSType, List<long>> insertedEntities, Dictionary<DMSType, HashSet<long>> newNeededGids)
        {
            GDAProxy gdaProxy = new GDAProxy("gdaQueryEndpoint");

            List<long> newBreakersGids;

            if (!insertedEntities.TryGetValue(DMSType.BREAKER, out newBreakersGids))
            {
                return;
            }

            List<ResourceDescription> rds = gdaProxy.GetExtentValues(DMSType.BREAKER, new List<ModelCode>() { ModelCode.PSR_MEASUREMENTS }, newBreakersGids);

            foreach (var rd in rds)
            {
                long discreteGid = rd.GetProperty(ModelCode.PSR_MEASUREMENTS).AsReferences().First();
                DMSType dmsType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(discreteGid);

                if (dmsType != DMSType.MEASUREMENTDISCRETE)
                {
                    continue;
                }

                if (!newNeededGids.ContainsKey(dmsType))
                {
                    newNeededGids[dmsType] = new HashSet<long>();
                }

                newNeededGids[dmsType].Add(discreteGid);
            }

        }

        protected override bool AdditionalProcessing(Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            CreateGraphs();

            return base.AdditionalProcessing(affectedEntities);
        }

        private bool CreateGraphs()
        {
            bool graphsCreated;
            // TODO CLEANUP STOPWATCH
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                graphsCreated = graphCreationStorage.Prepare(temporaryTransactionStorage as TopologyStorage);
            }
            catch
            {
                graphsCreated = false;
            }

            stopwatch.Stop();

            System.Console.WriteLine("Graphs created in {0} ms.", stopwatch.ElapsedMilliseconds);

            return graphsCreated;
        }
    }
}
