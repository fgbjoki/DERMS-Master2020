using Common.AbstractModel;
using Common.GDA;
using Common.PubSub;
using Common.PubSub.Messages;
using UIAdapter.Schema;

namespace UIAdapter.PubSub.DynamicHandlers
{
    public class SchemaBreakerStateChangedDynamichandler : BaseDynamicHandler<DiscreteRemotePointValuesChanged>
    {
        private IGraphSchemaController graphSchemaController;

        public SchemaBreakerStateChangedDynamichandler(IGraphSchemaController graphSchemaController)
        {
            this.graphSchemaController = graphSchemaController;
        }

        protected override void ProcessChanges(DiscreteRemotePointValuesChanged message)
        {
            foreach (var discreteChange in message.Changes)
            {
                Property currentValueProperty = discreteChange.GetProperty(ModelCode.MEASUREMENTDISCRETE_CURRENTVALUE);

                if (currentValueProperty == null)
                {
                    return;
                }

                int rawDiscreteValue = currentValueProperty.AsInt();

                graphSchemaController.ProcessDiscreteValueChanges(discreteChange.Id, rawDiscreteValue);
            }           
        }
    }
}
