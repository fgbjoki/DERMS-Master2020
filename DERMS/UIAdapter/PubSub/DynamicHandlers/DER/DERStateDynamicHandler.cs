using Common.PubSub;
using Common.PubSub.Messages;
using UIAdapter.TransactionProcessing.Storages.DERGroup;

namespace UIAdapter.PubSub.DynamicHandlers.DER
{
    public class DERStateDynamicHandler : BaseDynamicHandler<DERStateChanged>
    {
        private IDERGroupStorage derGroupStorage;

        public DERStateDynamicHandler(IDERGroupStorage derGroupStorage)
        {
            this.derGroupStorage = derGroupStorage;
        }

        protected override void ProcessChanges(DERStateChanged message)
        {
            foreach (var derState in message.DERStates)
            {
                derGroupStorage.UpdateDERState(derState.GlobalId, derState.ActivePower);
            }
        }
    }
}
