using Common.AbstractModel;
using Common.GDA;
using Common.PubSub;
using Common.PubSub.Messages;
using UIAdapter.Schema;

namespace UIAdapter.PubSub.DynamicHandlers
{
    public class SchemaBreakerStateChangedDynamichandler : BaseDynamicHandler<DiscreteRemotePointValueChanged>
    {
        private IGraphSchemaController graphSchemaController;

        public SchemaBreakerStateChangedDynamichandler(IGraphSchemaController graphSchemaController)
        {
            this.graphSchemaController = graphSchemaController;
        }

        protected override void ProcessChanges(DiscreteRemotePointValueChanged message)
        {
            Property currentValueProperty = message.GetProperty(ModelCode.MEASUREMENTDISCRETE_CURRENTVALUE);

            if (currentValueProperty == null)
            {
                return;
            }

            int rawDiscreteValue = currentValueProperty.AsInt();

            graphSchemaController.ProcessDiscreteValueChanges(message.Id, rawDiscreteValue);
        }
    }
}
