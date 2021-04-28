using UIAdapter.Helpers;
using UIAdapter.Schema.StateController;

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

        public bool Conduts { get { return conducts; } }

        public override EquipmentState GetEquipmentState()
        {
            return new BreakerEquipmentState(GlobalId)
            {
                DoesConduct = DoesConduct,
                Closed = Conduts,
                IsEnergized = IsEnergized
            };
        }
    }
}
