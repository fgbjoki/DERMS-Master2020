using Common.ComponentStorage.StorageItemCreator;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.GDA;
using UIAdapter.Model;

namespace UIAdapter.TransactionProcessing.StorageItemCreators
{
    public class DiscreteRemotePointStorageItemCreator : StorageItemCreator
    {
        public DiscreteRemotePointStorageItemCreator() : base(CreatePropertiesPerType())
        {
        }

        public override IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            DiscreteRemotePoint remotePoint = new DiscreteRemotePoint(rd.Id)
            {
                Name = rd.GetProperty(ModelCode.IDOBJ_NAME).AsString(),
                Address = rd.GetProperty(ModelCode.MEASUREMENT_ADDRESS).AsInt(),
                Value = rd.GetProperty(ModelCode.MEASUREMENTDISCRETE_CURRENTVALUE).AsInt(),
            };

            PopulateNormalOpenProeprty(rd, affectedEntities, remotePoint);

            return remotePoint;
        }

        private static void PopulateNormalOpenProeprty(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities, DiscreteRemotePoint remotePoint)
        {
            long psrGid = rd.GetProperty(ModelCode.MEASUREMENT_PSR).AsReference();

            if (psrGid != 0)
            {
                List<ResourceDescription> switches = affectedEntities[DMSType.BREAKER];

                foreach (var switchRd in switches)
                {
                    if (switchRd.Id == psrGid)
                    {
                        remotePoint.NormalValue = switchRd.GetProperty(ModelCode.SWITCH_NORMALOPEN).AsBool() == true ? 1 : 0;
                    }
                }
            }
        }

        private static Dictionary<ModelCode, List<ModelCode>> CreatePropertiesPerType()
        {
            return new Dictionary<ModelCode, List<ModelCode>>()
            {
                {   ModelCode.MEASUREMENTDISCRETE,
                    new List<ModelCode>()
                    {
                        ModelCode.IDOBJ_NAME,
                        ModelCode.MEASUREMENT_PSR,
                        ModelCode.MEASUREMENT_ADDRESS,
                        ModelCode.MEASUREMENTDISCRETE_CURRENTVALUE
                    }
                },

                {   ModelCode.BREAKER,
                    new List<ModelCode>()
                    {
                        ModelCode.SWITCH_NORMALOPEN
                    }
                }
            };
        }
    }
}
