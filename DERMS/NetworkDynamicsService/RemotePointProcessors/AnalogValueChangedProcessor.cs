using NetworkDynamicsService.Model.RemotePoints;
using Common.ComponentStorage;
using Common.GDA;
using NServiceBus;
using Common.PubSub;
using Common.PubSub.Messages;
using Common.AbstractModel;
using Common.Logger;

namespace NetworkDynamicsService.RemotePointProcessors
{
    public class AnalogValueChangedProcessor : ValueChangedProcessor<AnalogRemotePoint>
    {
        public AnalogValueChangedProcessor(IStorage<AnalogRemotePoint> storage, IDynamicPublisher publisher) : base(storage, publisher)
        {
        }

        protected override ResourceDescription ApplyChanges(AnalogRemotePoint remotePoint, ushort rawValue)
        {
            AnalogRemotePointValueChanged changes = new AnalogRemotePointValueChanged() { Id = remotePoint.GlobalId };

            float fieldValue = ReadFieldValue(rawValue);

            if (remotePoint.CurrentValue != fieldValue)
            {
                Logger.Instance.Log($"[{GetType()}] Analog remote point (gid: 0x{remotePoint.GlobalId:X16}) value changed to {fieldValue}");

                remotePoint.CurrentValue = fieldValue;
                changes.AddProperty(new Property(ModelCode.MEASUREMENTANALOG_CURRENTVALUE, fieldValue));
            }

            return changes;
        }

        protected override IEvent GetPublication(ResourceDescription changes)
        {
            return changes as AnalogRemotePointValueChanged;
        }

        protected override bool HasValueChanged(AnalogRemotePoint remotePoint, ushort rawValue)
        {
            float fieldValue = ReadFieldValue(rawValue);

            return remotePoint.CurrentValue != fieldValue;
        }

        private float ReadFieldValue(ushort rawValue)
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
