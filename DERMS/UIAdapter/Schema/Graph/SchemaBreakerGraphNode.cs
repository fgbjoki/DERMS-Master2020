using UIAdapter.Helpers;

namespace UIAdapter.Schema.Graph
{
    public class SchemaBreakerGraphNode : SchemaGraphNode
    {
        private bool conducts;

        public SchemaBreakerGraphNode(long globalId) : base(globalId)
        {
        }

        public override bool DoesConduct
        {
            get
            {
                return base.DoesConduct & conducts;
            }
        }

        public void ChangeBreakerState(BreakerState breakerState)
        {
            if (breakerState == BreakerState.CLOSED)
            {
                conducts = true;
            }
            else
            {
                conducts = false;
            }
        }
    }
}
