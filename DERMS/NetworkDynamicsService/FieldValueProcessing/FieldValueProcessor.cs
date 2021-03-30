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

namespace NetworkDynamicsService.FieldValueProcessing
{
    public class FieldValueProcessor : IFieldValuesProcessing
    {
        private Dictionary<DMSType, IValueChangedProcessor> remotePointProcessors;

        public FieldValueProcessor(IStorage<AnalogRemotePoint> analogStorage, IStorage<DiscreteRemotePoint> discreteStorage, IDynamicPublisher dynamicPublisher)
        {
            remotePointProcessors = new Dictionary<DMSType, IValueChangedProcessor>()
            {
                { DMSType.MEASUREMENTANALOG, new AnalogValueChangedProcessor(analogStorage, dynamicPublisher) },
                { DMSType.MEASUREMENTDISCRETE, new DiscreteValueChangedProcessor(discreteStorage, dynamicPublisher) }
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
                Logger.Instance.Log($"[{GetType()}] Cannot find processor for processing DMSType: {dmsType}. Skipping processing!");
                return;
            }

            foreach (var fieldValue in fieldValues)
            {
                valueChangedProcessor.ProcessChangedValue(fieldValue);
            }
        }
    }
}
