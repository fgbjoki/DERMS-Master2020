using Common.ComponentStorage.StorageItemCreator;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.GDA;
using FieldProcessor.Model;

namespace FieldProcessor.TransactionProcessing.StorageItemCreators
{
    public class DiscreteStorageItemCreator : StorageItemCreator
    {
        public DiscreteStorageItemCreator() : base(CreatePropertiesPerType())
        {
        }

        public override IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            SignalDirection signalDirection = (SignalDirection)rd.GetProperty(ModelCode.MEASUREMENT_DIRECTION).AsEnum();
            ushort address = (ushort)rd.GetProperty(ModelCode.MEASUREMENT_ADDRESS).AsInt();

            RemotePoint remotePoint = null;
            if (signalDirection == SignalDirection.Read)
            {
                remotePoint = new RemotePoint(rd.Id, address, RemotePointType.DiscreteInput);
            }
            else if (signalDirection == SignalDirection.ReadWrite)
            {
                remotePoint = new RemotePoint(rd.Id, address, RemotePointType.Coil);
            }

            return remotePoint;
        }

        private static Dictionary<ModelCode, List<ModelCode>> CreatePropertiesPerType()
        {
            return new Dictionary<ModelCode, List<ModelCode>>()
            {
                { ModelCode.MEASUREMENTDISCRETE, new List<ModelCode>()
                    {
                        ModelCode.IDOBJ_GID,
                        ModelCode.MEASUREMENT_ADDRESS,
                        ModelCode.MEASUREMENT_DIRECTION
                    }
                }
            };
        }
    }
}
