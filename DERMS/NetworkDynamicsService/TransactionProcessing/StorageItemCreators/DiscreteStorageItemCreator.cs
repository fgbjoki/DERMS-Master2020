using Common.ComponentStorage.StorageItemCreator;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.GDA;
using NetworkDynamicsService.Model.RemotePoints;
using System.Linq;
using System;

namespace NetworkDynamicsService.TransactionProcessing.StorageItemCreators
{
    public class DiscreteStorageItemCreator : StorageItemCreator
    {
        public DiscreteStorageItemCreator() : base()
        {
        }

        public override IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            int normalValue;
            int currentValue;
            int remotePointAddress;
            DiscreteRemotePointType remotePointType;

            if (!PopulateCurrentValue(rd, out currentValue))
            {
                // log
                return null;
            }

            if (!PopulateRemotePointDirection(rd, out remotePointType))
            {
                // log
                return null;
            }

            if (!PopulateRemotePointAddress(rd, out remotePointAddress))
            {
                // log
                return null;
            }

            if (!PopulateNormalValue(affectedEntities, rd, out normalValue))
            {
                // log
                return null;
            }

            return new DiscreteRemotePoint(rd.Id, remotePointAddress, currentValue, normalValue, remotePointType);
        }

        private bool PopulateNormalValue(Dictionary<DMSType, List<ResourceDescription>> affectedEntities, ResourceDescription rd, out int normalValue)
        {
            normalValue = 0;

            Property psrProperty = rd.GetProperty(ModelCode.MEASUREMENT_PSR);

            if (psrProperty == null)
            {
                return false;
            }

            long breakerGid = psrProperty.AsReference();

            List<ResourceDescription> breakers;

            if (!affectedEntities.TryGetValue(DMSType.BREAKER, out breakers))
            {
                return false;
            }

            ResourceDescription neededBreaker = breakers.FirstOrDefault(x => x.Id == breakerGid);

            if (neededBreaker == null)
            {
                return false;
            }

            Property normalValueProperty = neededBreaker.GetProperty(ModelCode.SWITCH_NORMALOPEN);

            if (normalValueProperty == null)
            {
                return false;
            }

            normalValue = normalValueProperty.AsBool() ? 1 : 0;
            return true;
        }

        private bool PopulateRemotePointDirection(ResourceDescription rd, out DiscreteRemotePointType discreteRemotePointType)
        {
            discreteRemotePointType = DiscreteRemotePointType.Coil;

            Property directionProperty = rd.GetProperty(ModelCode.MEASUREMENT_DIRECTION);

            if (directionProperty == null)
            {
                return false;
            }

            SignalDirection remotePointDirection = (SignalDirection)directionProperty.AsEnum();

            if (remotePointDirection == SignalDirection.Read)
            {
                discreteRemotePointType = DiscreteRemotePointType.DiscreteInput;
            }
            else if (remotePointDirection == SignalDirection.ReadWrite || remotePointDirection == SignalDirection.Write)
            {
                discreteRemotePointType = DiscreteRemotePointType.Coil;
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

        private bool PopulateCurrentValue(ResourceDescription rd, out int value)
        {
            value = 0;

            Property currentValueProperty = rd.GetProperty(ModelCode.MEASUREMENTDISCRETE_CURRENTVALUE);

            if (currentValueProperty == null)
            {
                return false;
            }

            value = currentValueProperty.AsInt();
            return true;
        }

        public override Dictionary<DMSType, List<ModelCode>> GetNeededProperties()
        {
            return new Dictionary<DMSType, List<ModelCode>>()
            {
                { DMSType.MEASUREMENTDISCRETE, new List<ModelCode>()
                    {
                        ModelCode.MEASUREMENTDISCRETE_CURRENTVALUE,
                        ModelCode.MEASUREMENTDISCRETE_MAXVALUE,
                        ModelCode.MEASUREMENTDISCRETE_MINVALUE,
                        ModelCode.MEASUREMENT_ADDRESS,
                        ModelCode.MEASUREMENT_DIRECTION,
                        ModelCode.MEASUREMENT_PSR
                    }
                },
                { DMSType.BREAKER, new List<ModelCode>()
                    {
                        ModelCode.SWITCH_NORMALOPEN
                    }
                },
            };
        }
    }
}
