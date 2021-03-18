using CalculationEngine.Graphs.GraphProcessors;
using CalculationEngine.Model.Topology.Graph.Schema;
using System.Collections.Generic;

namespace CalculationEngine.Schema
{
    public class SchemaRepresentation : BaseGraphProcessor<ISchemaGraph>
    {
        protected override IEnumerable<long> GetRootsGlobalId(ISchemaGraph graph)
        {
            return new List<long>(1) { graph.GetRoot().Item };
        }
    }
}
