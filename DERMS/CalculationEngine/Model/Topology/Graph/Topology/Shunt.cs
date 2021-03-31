using Common.AbstractModel;

namespace CalculationEngine.Model.Topology.Graph.Topology
{
    public class Shunt
    {
        public Shunt(long globalId)
        {
            GlobalId = globalId;
            DMSType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(globalId);
        }

        public long GlobalId { get; private set; }

        public DMSType DMSType { get; private set; }
    }
}
