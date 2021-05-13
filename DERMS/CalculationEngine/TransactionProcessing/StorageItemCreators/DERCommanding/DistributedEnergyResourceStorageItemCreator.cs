using Common.ComponentStorage.StorageItemCreator;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.GDA;
using CalculationEngine.Model.DERCommanding;

namespace CalculationEngine.TransactionProcessing.StorageItemCreators.DERCommanding
{
    public abstract class DistributedEnergyResourceStorageItemCreator<T> : StorageItemCreator
        where T : DistributedEnergyResource
    {
        public override IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            T der = InstantiateDER(rd);
            PopulateDERProperties(der, rd, affectedEntities);

            return der;
        }

        protected virtual void PopulateDERProperties(T der, ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            der.NominalPower = rd.GetProperty(ModelCode.DER_NOMINALPOWER).AsFloat();
        }

        protected abstract T InstantiateDER(ResourceDescription rd);

        public override Dictionary<DMSType, List<ModelCode>> GetNeededProperties()
        {
            return new Dictionary<DMSType, List<ModelCode>>()
            {
                //{ DMSType.WINDGENERATOR, new List<ModelCode>() { ModelCode.DER_NOMINALPOWER } },
                { DMSType.ENERGYSTORAGE, new List<ModelCode>() { ModelCode.DER_NOMINALPOWER } },
                //{ DMSType.SOLARGENERATOR, new List<ModelCode>() { ModelCode.DER_NOMINALPOWER } }
                { DMSType.MEASUREMENTANALOG, new List<ModelCode>() { ModelCode.MEASUREMENTANALOG_CURRENTVALUE } }
            };
        }
    }
}
