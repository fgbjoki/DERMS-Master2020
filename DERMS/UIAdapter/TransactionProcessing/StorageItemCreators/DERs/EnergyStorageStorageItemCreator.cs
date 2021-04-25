using Common.ComponentStorage.StorageItemCreator;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.GDA;
using UIAdapter.Model.DERs;
using System;

namespace UIAdapter.TransactionProcessing.StorageItemCreators.DERs
{
    public class EnergyStorageStorageItemCreator : StorageItemCreator
    {
        public EnergyStorageStorageItemCreator()
        {
        }

        public override IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            EnergyStorage energyStorage = new EnergyStorage(rd.Id);
            energyStorage.Capacity = rd.GetProperty(ModelCode.ENERGYSTORAGE_CAPACITY).AsFloat();
            energyStorage.NominalPower = rd.GetProperty(ModelCode.DER_NOMINALPOWER).AsFloat();

            return energyStorage;
        }

        public override Dictionary<DMSType, List<ModelCode>> GetNeededProperties()
        {
            return new Dictionary<DMSType, List<ModelCode>>()
            {
                {
                    DMSType.ENERGYSTORAGE,
                    new List<ModelCode>()
                    {
                        ModelCode.PSR_MEASUREMENTS,
                        ModelCode.DER_NOMINALPOWER,
                        ModelCode.ENERGYSTORAGE_CAPACITY,
                        ModelCode.ENERGYSTORAGE_GENERATOR
                    }
                },
            };
        }
    }
}
