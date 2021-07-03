using System.Collections.Generic;
using Core.Common.AbstractModel;
using Core.Common.GDA;
using Core.Common.Transaction.Models;
using Core.Common.Transaction.Models.FEP.FEPStorage;
using Core.Common.Transaction.Storage;
using Core.Common.Transaction.StorageItemCreator;
using FEPStorage.Model;

namespace FieldProcessor.TransactionProcessing.StorageItemCreators
{
    public class DiscreteStorageItemCreator : StorageItemCreator
    {
        public DiscreteStorageItemCreator() : base()
        {
        }

        public override IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            SignalDirection signalDirection = (SignalDirection)rd.GetProperty(ModelCode.MEASUREMENT_DIRECTION).AsEnum();
            ushort address = (ushort)rd.GetProperty(ModelCode.MEASUREMENT_ADDRESS).AsInt();

            DiscreteRemotePoint remotePoint = null;
            if (signalDirection == SignalDirection.Read)
            {
                remotePoint = new DiscreteRemotePoint(rd.Id, address, RemotePointType.DiscreteInput);
            }
            else if (signalDirection == SignalDirection.ReadWrite)
            {
                remotePoint = new DiscreteRemotePoint(rd.Id, address, RemotePointType.Coil);
            }

            return remotePoint;
        }

        public override Dictionary<DMSType, List<ModelCode>> GetNeededProperties()
        {
            return new Dictionary<DMSType, List<ModelCode>>()
            {
                { DMSType.MEASUREMENTDISCRETE, new List<ModelCode>()
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
