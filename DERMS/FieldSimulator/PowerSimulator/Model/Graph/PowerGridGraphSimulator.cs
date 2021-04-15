using FieldSimulator.PowerSimulator.Model.Graph.TopologyGraphCreator;
using FieldSimulator.PowerSimulator.Storage;

namespace FieldSimulator.PowerSimulator.Model.Graph
{
    public class PowerGridGraphSimulator
    {
        private TopologyGraph topologyGraph;
        private GraphCreatorWrapper graphCreator;
        private PowerGridSimulatorStorage storage;

        public PowerGridGraphSimulator(PowerGridSimulatorStorage storage)
        {
            this.storage = storage;
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
