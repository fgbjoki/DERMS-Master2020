using Common.ServiceInterfaces.NetworkDynamicsService;
using System.Collections.Generic;
using System.Linq;
using Common.SCADA;
using Common.AbstractModel;
using NetworkDynamicsService.RemotePointProcessors;
using Common.ComponentStorage;
using NetworkDynamicsService.Model.RemotePoints;
using Common.PubSub;
using Common.Logger;
using NServiceBus;

namespace NetworkDynamicsService.FieldValueProcessing
{
    public class FieldValueProcessor : IFieldValuesProcessing
    {
        private Dictionary<DMSType, IValueChangedProcessor> remotePointProcessors;
        private IDynamicPublisher dynamicPublisher;

        public FieldValueProcessor(IStorage<AnalogRemotePoint> analogStorage, IStorage<DiscreteRemotePoint> discreteStorage, IDynamicPublisher dynamicPublisher)
        {
            this.dynamicPublisher = dynamicPublisher;

            remotePointProcessors = new Dictionary<DMSType, IValueChangedProcessor>()
            {
                { DMSType.MEASUREMENTANALOG, new AnalogValueChangedProcessor(analogStorage) },
                { DMSType.MEASUREMENTDISCRETE, new DiscreteValueChangedProcessor(discreteStorage) }
            };
        }

        public void ProcessFieldValues(IEnumerable<RemotePointFieldValue> fieldValues)
        {
            if (fieldValues.Count() == 0)
            {
                return;
            }

            DMSType dmsType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(fieldValues.First().GlobalId);

            IValueChangedProcessor valueChangedProcessor;
            if (!remotePointProcessors.TryGetValue(dmsType, out valueChangedProcessor))
            {
                Logger.Instance.Log($"[{GetType().Name}] Cannot find processor for processing DMSType: {dmsType}. Skipping processing!");
                return;
            }

            IEvent publication = valueChangedProcessor.ProcessChangedValue(fieldValues);
            if (publication != null)
            {
                dynamicPublisher.Publish(publication);
            }
        }
    }
}
