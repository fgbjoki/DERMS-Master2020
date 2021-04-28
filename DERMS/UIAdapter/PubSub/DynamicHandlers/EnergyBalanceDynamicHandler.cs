using Common.PubSub;
using Common.PubSub.Messages;
using UIAdapter.Schema;

namespace UIAdapter.PubSub.DynamicHandlers
{
    public class EnergyBalanceDynamicHandler : BaseDynamicHandler<EnergyBalanceChanged>
    {
        private IGraphSchemaController graphSchemaController;

        public EnergyBalanceDynamicHandler(IGraphSchemaController graphSchemaController)
        {
            this.graphSchemaController = graphSchemaController;
        }

        protected override void ProcessChanges(EnergyBalanceChanged message)
        {
            if (message == null)
            {
                return;
            }

            graphSchemaController.ProcessEnergyBalanceChange(message);
        }
    }
}
