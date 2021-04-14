using Common.AbstractModel;
using FieldSimulator.PowerSimulator.Model.Graph.GraphManipulators;
using FieldSimulator.PowerSimulator.Model.Graph.TopologyGraphCreator;
using System.Linq;

namespace FieldSimulator.PowerSimulator.Model.Graph
{
    public class GraphCreatorWrapper
    {
        private ConnectivityGraphCreator.ConnectivityGraphCreator connectivityGraphCreator;
        private TopologyGraphCreator.TopologyGraphCreator topologyGraphCreator;

        public GraphCreatorWrapper()
        {
            GraphBranchManipulator graphBranchManipulator = new GraphBranchManipulator();
            ModelResourcesDesc modelRescDesc = new ModelResourcesDesc();
            connectivityGraphCreator = new ConnectivityGraphCreator.ConnectivityGraphCreator(graphBranchManipulator, modelRescDesc);

            topologyGraphCreator = new TopologyGraphCreator.TopologyGraphCreator(new TopologyGraphBranchManipulator(), new TopologyBreakerGraphBranchManipulator());
        }

        public TopologyGraph CreateGraph(EntityStorage entityStorage)
        {
            var connectivityGraphs = connectivityGraphCreator.CreateGraph(entityStorage);

            return topologyGraphCreator.CreateGraph(connectivityGraphs.First()).First() as TopologyGraph;
        }
    }
}
