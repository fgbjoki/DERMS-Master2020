using CalculationEngine.CommonComponents;
using Common.AbstractModel;
using Common.GDA;
using Common.Logger;
using Common.PubSub;
using Common.PubSub.Messages;
using System;
using System.Collections.Generic;

namespace CalculationEngine.PubSub.DynamicHandlers
{
    public class CalculatingUnitAnalogValueChanged : BaseDynamicHandler<AnalogRemotePointValuesChanged>
    {
        private ITopologyDependentComponent topologyDependentComponent;

        public CalculatingUnitAnalogValueChanged(ITopologyDependentComponent topologyDependentComponent)
        {
            this.topologyDependentComponent = topologyDependentComponent;
        }

        protected override void ProcessChanges(AnalogRemotePointValuesChanged message)
        {
            List<Tuple<long, float>> changes = new List<Tuple<long, float>>(message.Changes.Count);

            foreach (var analogChange in message.Changes)
            {
                Property currentValueProperty = analogChange.GetProperty(ModelCode.MEASUREMENTANALOG_CURRENTVALUE);

                if (currentValueProperty == null)
                {
                    Logger.Instance.Log($"[{this.GetType()}] Cannot find value property for analog measurement with gid: {analogChange.Id:X16}. Skipping further processing!");
                    return;
                }

                float value = currentValueProperty.AsFloat();


                changes.Add(new Tuple<long, float>(analogChange.Id, value));
            }           
            topologyDependentComponent.ProcessAnalogChanges(changes);
        }
    }
}
