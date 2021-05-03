using NetworkDynamicsService.Model.RemotePoints;
using Common.ComponentStorage;
using Common.GDA;
using NServiceBus;
using Common.AbstractModel;
using Common.Logger;
using Common.PubSub.Messages;
using System.Collections.Generic;

namespace NetworkDynamicsService.RemotePointProcessors
{
    public class DiscreteValueChangedProcessor : ValueChangedProcessor<DiscreteRemotePoint>
    {
        public DiscreteValueChangedProcessor(IStorage<DiscreteRemotePoint> storage) : base(storage)
        {
        }

        protected override ResourceDescription ApplyChanges(DiscreteRemotePoint remotePoint, int rawValue)
        {
            ResourceDescription changes = new ResourceDescription() { Id = remotePoint.GlobalId };

            int fieldValue = ReadFieldValue(rawValue);

            if (remotePoint.CurrentValue != fieldValue)
            {
                remotePoint.CurrentValue = fieldValue;
                changes.AddProperty(new Property(ModelCode.MEASUREMENTDISCRETE_CURRENTVALUE, fieldValue));

                remotePoint.DOMManipulations++;
                changes.AddProperty(new Property(ModelCode.MEASUREMENTDISCRETE_DOM, remotePoint.DOMManipulations));

                Logger.Instance.Log($"[{GetType()}] Discrete remote point (gid: 0x{remotePoint.GlobalId:X16}) value changed to {fieldValue}, DOM: {remotePoint.DOMManipulations}");
            }

            return changes;
        }

        protected override List<ResourceDescription> CreatePublication()
        {
            return new DiscreteRemotePointValuesChanged();
        }

        protected override IEvent GetPublication(List<ResourceDescription> publicationChanges)
        {
            return publicationChanges as IEvent;
        }

        protected override bool HasValueChanged(DiscreteRemotePoint remotePoint, int rawValue)
        {
            int fieldValue = ReadFieldValue(rawValue);

            return remotePoint.CurrentValue != fieldValue;
        }

        private int ReadFieldValue(int rawValue)
        {
            if (rawValue == 0xFF00)
            {
                return 1;
            }
            else if (rawValue == 0x00FF)
            {
                return 0;
            }

            return rawValue & 0x0001;
        }
    }
}
