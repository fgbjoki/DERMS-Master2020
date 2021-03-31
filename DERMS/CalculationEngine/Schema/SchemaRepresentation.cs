using CalculationEngine.Graphs.GraphProcessors;
using CalculationEngine.Model.Topology.Graph.Schema;
using Common.PubSub;
using Common.ServiceInterfaces.CalculationEngine;
using System.Collections.Generic;
using System.Linq;
using Common.DataTransferObjects.CalculationEngine;

namespace CalculationEngine.Schema
{
    public class SchemaRepresentation : BaseGraphProcessor<ISchemaGraph>, ISchemaRepresentation
    {
        private SchemaMessageConverter graphConverter;

        public SchemaRepresentation()
        {
            graphConverter = new SchemaMessageConverter();
        }

        public SchemaGraphChanged GetSchema(long sourceId)
        {
            ISchemaGraph graph;

            if (!graphs.TryGetValue(sourceId, out graph))
            {
                return null;
            }

            return graphConverter.Convert(graph);
        }

        public IEnumerable<long> GetSchemaSources()
        {
            return graphs.Keys.ToList();
        }

        protected override IEnumerable<long> GetRootsGlobalId(ISchemaGraph graph)
        {
            return new List<long>(1) { graph.GetRoot().Item };
        }
    }
}
