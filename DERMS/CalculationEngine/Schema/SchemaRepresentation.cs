using CalculationEngine.Graphs.GraphProcessors;
using CalculationEngine.Model.Topology.Graph;
using CalculationEngine.Model.Topology.Graph.Schema;
using System.Collections.Generic;

namespace CalculationEngine.Schema
{
    public class SchemaRepresentation : BaseGraphProcessor<ISingleRootGraph<SchemaGraphNode>>
    {
        protected override IEnumerable<long> GetRootsGlobalId(ISingleRootGraph<SchemaGraphNode> graph)
        {
            return new List<long>(1) { graph.GetRoot().Item };
        }
    }
}
