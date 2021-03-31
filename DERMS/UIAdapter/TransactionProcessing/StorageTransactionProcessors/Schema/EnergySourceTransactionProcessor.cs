using Common.ComponentStorage;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage.StorageItemCreator;
using UIAdapter.Model.Schema;
using Common.GDA;
using System.Linq;

namespace UIAdapter.TransactionProcessing.StorageTransactionProcessors.Schema
{
    public class EnergySourceTransactionProcessor : StorageTransactionProcessor<EnergySource>
    {
        public EnergySourceTransactionProcessor(IStorage<EnergySource> storage, Dictionary<DMSType, IStorageItemCreator> storageItemCreators) : base(storage, storageItemCreators)
        {
        }

        protected override List<DMSType> GetPrimaryTypes()
        {
            return new List<DMSType>(1) { DMSType.ENERGYSOURCE };
        }

        protected override void AddAdditionalEntities(Dictionary<DMSType, List<long>> insertedEntities, Dictionary<DMSType, HashSet<long>> newNeededGids)
        {
            base.AddAdditionalEntities(insertedEntities, newNeededGids);

            List<long> energySources;

            if (!insertedEntities.TryGetValue(DMSType.ENERGYSOURCE, out energySources))
            {
                return;
            }

            GDAProxy gdaProxy = new GDAProxy("gdaQueryEndpoint");

            List<ResourceDescription> energySourcesRd = gdaProxy.GetExtentValues(DMSType.ENERGYSOURCE, new List<ModelCode>() { ModelCode.EQUIPMENT_EQCONTAINER }, energySources);

            if (!newNeededGids.ContainsKey(DMSType.SUBSTATION))
            {
                newNeededGids[DMSType.SUBSTATION] = new HashSet<long>();
            }

            foreach (var substationGid in energySourcesRd.Select(x => x.GetProperty(ModelCode.EQUIPMENT_EQCONTAINER).AsReference()))
            {
                newNeededGids[DMSType.SUBSTATION].Add(substationGid);
            }
        }
    }
}
