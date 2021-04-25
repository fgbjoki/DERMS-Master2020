using Common.ComponentStorage.StorageItemCreator;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.GDA;
using FieldProcessor.Model;
using System;

namespace FieldProcessor.TransactionProcessing.StorageItemCreators
{
    public class AnalogStorageItemCreator : StorageItemCreator
    {
        public AnalogStorageItemCreator()
        {
        }

        public override IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            SignalDirection signalDirection = (SignalDirection)rd.GetProperty(ModelCode.MEASUREMENT_DIRECTION).AsEnum();
            ushort address = (ushort)rd.GetProperty(ModelCode.MEASUREMENT_ADDRESS).AsInt();

            RemotePoint remotePoint = null;
            if (signalDirection == SignalDirection.Read)
            {
                remotePoint = new RemotePoint(rd.Id, address, RemotePointType.InputRegister);
            }
            else if (signalDirection == SignalDirection.ReadWrite)
            {
                remotePoint = new RemotePoint(rd.Id, address, RemotePointType.HoldingRegister);
            }

            return remotePoint;
        }

        public override Dictionary<DMSType, List<ModelCode>> GetNeededProperties()
        {
            return new Dictionary<DMSType, List<ModelCode>>()
            {
                { DMSType.MEASUREMENTANALOG, new List<ModelCode>()
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
