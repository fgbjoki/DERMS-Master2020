using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.GDA;
using UIAdapter.Model.DERGroup;
using Common.ComponentStorage.StorageItemCreator;

namespace UIAdapter.TransactionProcessing.StorageItemCreators.DERGroup
{
    public class EnergyStorageStorageItemCreator : StorageItemCreator
    {
        public EnergyStorageStorageItemCreator()
        {
        }

        public override IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            EnergyStorage energyStorage = new EnergyStorage(rd.Id);
            energyStorage.NominalPower = rd.GetProperty(ModelCode.DER_NOMINALPOWER).AsFloat();
            energyStorage.Name = rd.GetProperty(ModelCode.IDOBJ_NAME).AsString();

            return energyStorage;
        }

        public override Dictionary<DMSType, List<ModelCode>> GetNeededProperties()
        {
            return new Dictionary<DMSType, List<ModelCode>>()
            {
                {
                    DMSType.ENERGYSTORAGE,
                    new List<ModelCode>() { ModelCode.IDOBJ_NAME, ModelCode.DER_NOMINALPOWER, ModelCode.ENERGYSTORAGE_CAPACITY }
                },
            };
        }
    }
}
