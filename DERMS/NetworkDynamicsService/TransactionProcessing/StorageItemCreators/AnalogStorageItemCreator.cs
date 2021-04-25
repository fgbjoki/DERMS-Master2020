using Common.ComponentStorage.StorageItemCreator;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.GDA;
using NetworkDynamicsService.Model.RemotePoints;
using System;

namespace NetworkDynamicsService.TransactionProcessing.StorageItemCreators
{
    public class AnalogStorageItemCreator : StorageItemCreator
    {
        public AnalogStorageItemCreator() : base()
        {
        }

        public override IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            int remotePointAddress;
            long globalId = rd.Id;
            float currentValue;
            float minValue;
            float maxValue;
            AnalogRemotePointType analogRemotePointType;

            if (!PopulateValueProperties(rd, out currentValue, out minValue, out maxValue))
            {
                // log
                return null;
            }

            if (!PopulateRemotePointAddress(rd, out remotePointAddress))
            {
                // log
                return null;
            }

            if (!PopulateRemotePointDirection(rd, out analogRemotePointType))
            {
                // log
                return null;
            }

            return new AnalogRemotePoint(globalId, remotePointAddress, currentValue, minValue, maxValue, analogRemotePointType);
        }

        private bool PopulateRemotePointDirection(ResourceDescription rd, out AnalogRemotePointType analogRemotePointType)
        {
            analogRemotePointType = AnalogRemotePointType.HoldingRegister;

            Property directionProperty = rd.GetProperty(ModelCode.MEASUREMENT_DIRECTION);

            if (directionProperty == null)
            {
                return false;
            }

            SignalDirection remotePointDirection = (SignalDirection)directionProperty.AsEnum();

            if (remotePointDirection == SignalDirection.Read)
            {
                analogRemotePointType = AnalogRemotePointType.InputRegister;
            }
            else if (remotePointDirection == SignalDirection.ReadWrite || remotePointDirection == SignalDirection.Write)
            {
                analogRemotePointType = AnalogRemotePointType.HoldingRegister;
            }

            return true;
        }

        private bool PopulateRemotePointAddress(ResourceDescription rd, out int remotePointAddress)
        {
            remotePointAddress = 0;

            Property addressProperty = rd.GetProperty(ModelCode.MEASUREMENT_ADDRESS);

            if (addressProperty == null)
            {
                return false;
            }

            remotePointAddress = addressProperty.AsInt();
            return true;
        }

        private bool PopulateValueProperties(ResourceDescription rd, out float currentValue, out float minValue, out float maxValue)
        {
            minValue = 0;
            currentValue = 0;
            maxValue = 0;

            return TryGetFloatProperty(rd, ModelCode.MEASUREMENTANALOG_CURRENTVALUE, out currentValue) && TryGetFloatProperty(rd, ModelCode.MEASUREMENTANALOG_MAXVALUE, out maxValue) &&
                TryGetFloatProperty(rd, ModelCode.MEASUREMENTANALOG_MINVALUE, out minValue);
        }

        private bool TryGetFloatProperty(ResourceDescription rd, ModelCode property, out float value)
        {
            value = 0;

            Property floatProperty = rd.GetProperty(property);

            if (floatProperty == null)
            {
                return false;
            }

            value = floatProperty.AsFloat();
            return true;
        }

        public override Dictionary<DMSType, List<ModelCode>> GetNeededProperties()
        {
            return new Dictionary<DMSType, List<ModelCode>>()
            {
                { DMSType.MEASUREMENTANALOG, new List<ModelCode>()
                    {
                        ModelCode.MEASUREMENTANALOG_CURRENTVALUE,
                        ModelCode.MEASUREMENTANALOG_MAXVALUE,
                        ModelCode.MEASUREMENTANALOG_MINVALUE,
                        ModelCode.MEASUREMENT_ADDRESS,
                        ModelCode.MEASUREMENT_DIRECTION
                    }
                }
            };
        }
    }
}
