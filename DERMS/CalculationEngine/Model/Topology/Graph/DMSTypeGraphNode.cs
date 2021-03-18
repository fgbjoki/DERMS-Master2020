using Common.AbstractModel;

namespace CalculationEngine.Model.Topology.Graph
{
    public abstract class DMSTypeGraphNode : GenericGraphNode<long>
    {
        public DMSTypeGraphNode(long globalId) : base(globalId)
        {
            DMSType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(globalId);
        }

        public DMSType DMSType { get; private set; }
    }
}
