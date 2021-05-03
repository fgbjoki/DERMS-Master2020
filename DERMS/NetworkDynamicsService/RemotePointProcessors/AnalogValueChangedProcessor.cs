using NetworkDynamicsService.Model.RemotePoints;
using Common.ComponentStorage;
using Common.GDA;
using Common.AbstractModel;
using Common.Logger;
using NServiceBus;
using System.Collections.Generic;
using Common.PubSub.Messages;

namespace NetworkDynamicsService.RemotePointProcessors
{
    public class AnalogValueChangedProcessor : ValueChangedProcessor<AnalogRemotePoint>
    {
        public AnalogValueChangedProcessor(IStorage<AnalogRemotePoint> storage) : base(storage)
        {
        }

        protected override ResourceDescription ApplyChanges(AnalogRemotePoint remotePoint, int rawValue)
        {
            ResourceDescription changes = new ResourceDescription() { Id = remotePoint.GlobalId };

            float fieldValue = ReadFieldValue(rawValue);

            if (remotePoint.CurrentValue != fieldValue)
            {
                Logger.Instance.Log($"[{GetType()}] Analog remote point (gid: 0x{remotePoint.GlobalId:X16}) value changed to {fieldValue}");

                remotePoint.CurrentValue = fieldValue;
                changes.AddProperty(new Property(ModelCode.MEASUREMENTANALOG_CURRENTVALUE, fieldValue));
            }

            return changes;
        }

        protected override List<ResourceDescription> CreatePublication()
        {
            return new AnalogRemotePointValuesChanged();
        }

        protected override IEvent GetPublication(List<ResourceDescription> publicationChanges)
        {
            return publicationChanges as IEvent;
        }

        protected override bool HasValueChanged(AnalogRemotePoint remotePoint, int rawValue)
        {
            float fieldValue = ReadFieldValue(rawValue);

            return remotePoint.CurrentValue != fieldValue;
        }

        private float ReadFieldValue(int rawValue)
        {
            float value;
            unsafe
            {
                float *fieldValue = (float*)(&rawValue);
                value = *fieldValue;
            }

            return value;
        }
    }
}
