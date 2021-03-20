using CalculationEngine.Graphs.GraphProcessors;
using CalculationEngine.Model.Topology.Graph.Schema;
using Common.Logger;
using Common.PubSub;
using Common.PubSub.Messages;
using System.Collections.Generic;
using System.Threading;

namespace CalculationEngine.Schema
{
    public class SchemaRepresentation : BaseGraphProcessor<ISchemaGraph>
    {
        private IDynamicPublisher dynamicPublisher;
        private SchemaMessageConverter graphConverter;

        public SchemaRepresentation(IDynamicPublisher dynamicPublisher)
        {
            this.dynamicPublisher = dynamicPublisher;

            graphConverter = new SchemaMessageConverter();
        }

        public override bool AddGraph(ISchemaGraph graph)
        {
            bool isGraphAdded = base.AddGraph(graph);

            if (isGraphAdded)
            {
                try
                {
                    ThreadPool.QueueUserWorkItem(PublishNewSchema, graph);
                }
                catch
                {
                    Logger.Instance.Log($"[{this.GetType()}] Error queuing publication of schema graph! Schema will not be published!");
                }
            }

            return isGraphAdded;
        }

        protected override IEnumerable<long> GetRootsGlobalId(ISchemaGraph graph)
        {
            return new List<long>(1) { graph.GetRoot().Item };
        }

        private void PublishNewSchema(object parameter)
        {
            ISchemaGraph graph = (ISchemaGraph)parameter;

            SchemaGraphChanged message = graphConverter.Convert(graph);

            dynamicPublisher.Publish(message);
        }
    }
}
