using FieldSimulator.PowerSimulator.Model.Graph.TopologyGraphCreator;

namespace FieldSimulator.PowerSimulator.Model.Graph
{
    public class PowerGridGraphSimulator
    {
        private TopologyGraph topologyGraph;
        private GraphCreatorWrapper graphCreator;

        public PowerGridGraphSimulator()
        {
            graphCreator = new GraphCreatorWrapper();
        }

        public void CreateGraphs(EntityStorage entityStorage)
        {
            topologyGraph = graphCreator.CreateGraph(entityStorage);
        }

        private void PopulateCalculationPoints(EntityStorage entityStorage)
        {

        }
    }
}
