using CalculationEngine.Model.Topology.Graph.Topology;
using System.Collections.Generic;

namespace CalculationEngine.TopologyAnalysis.GraphTraversing
{
    public class TopologyGraphSourceFinder
    {
        public long FindSource(TopologyGraphNode node)
        {
            Queue<TopologyGraphNode> nodesToProcess = new Queue<TopologyGraphNode>();
            nodesToProcess.Enqueue(node);

            while (nodesToProcess.Count > 0)
            {
                TopologyGraphNode currentNode = nodesToProcess.Dequeue();

                // find if it's energysource
                if (currentNode.DMSType == Common.AbstractModel.DMSType.ENERGYSOURCE)
                {
                    return currentNode.Item;
                }

                // queue parents
                foreach (var parentBranch in currentNode.ParentBranches)
                {
                    TopologyGraphBranch topologyParentBranch = parentBranch as TopologyGraphBranch;

                    if (!topologyParentBranch.DoesBranchConduct())
                    {
                        continue;
                    }

                    TopologyGraphNode parentNode = parentBranch.UpStream as TopologyGraphNode;

                    nodesToProcess.Enqueue(parentNode);
                }
            }

            // Source node not found
            return 0;
        }
    }
}
