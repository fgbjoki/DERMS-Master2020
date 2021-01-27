using Common.ComponentStorage.StorageItemCreator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.GDA;
using UIAdapter.Model;

namespace UIAdapter.TransactionProcessing.StorageItemCreators
{
    public class AnalogRemotePointStorageItemCreator : StorageItemCreator
    {
        public AnalogRemotePointStorageItemCreator() : base(CreatePropertiesPerType())
        {
        }

        public override IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            AnalogRemotePoint remotePoint = new AnalogRemotePoint(rd.Id)
            {
                Name = rd.GetProperty(ModelCode.IDOBJ_NAME).AsString(),
                Address = rd.GetProperty(ModelCode.MEASUREMENT_ADDRESS).AsInt(),
                Value = rd.GetProperty(ModelCode.MEASUREMENTANALOG_CURRENTVALUE).AsFloat(),
            };

            return remotePoint;
        }

        private static Dictionary<ModelCode, List<ModelCode>> CreatePropertiesPerType()
        {
            return new Dictionary<ModelCode, List<ModelCode>>()
            {
                { ModelCode.MEASUREMENTANALOG,
                    new List<ModelCode>()
                    {
                        ModelCode.IDOBJ_NAME,
                        ModelCode.MEASUREMENT_ADDRESS,
                        ModelCode.MEASUREMENTANALOG_CURRENTVALUE
                    }
                }
            };
        }
    }
}
