using CalculationEngine.Model.Topology.Graph;
using Common.AbstractModel;
using System.Collections.Generic;
using System.Linq;

namespace CalculationEngine.Graphs.SchemaGraphCreation.Helpers
{
    public class ConnectivityBreakerNodeFinder
    {
        public long FindConnectivityBreaker(DMSTypeGraphNode root)
        {
            long breakerGID = 0;

            Queue<DMSTypeGraphNode> nodesToProcess = new Queue<DMSTypeGraphNode>();
            nodesToProcess.Enqueue(root);

            while (nodesToProcess.Count > 0)
            {
                DMSTypeGraphNode currentNode = nodesToProcess.Dequeue();

                if (!ShouldNodeBeProcessed(currentNode))
                {
                    EnqueueChildren(currentNode, nodesToProcess);
                    continue;
                }

                if (CanBreakerReachBothSources(currentNode))
                {
                    return currentNode.Item;
                }
            }

            return breakerGID;
        }

        private bool CanBreakerReachBothSources(DMSTypeGraphNode breakerNode)
        {
            foreach (var parent in breakerNode.ParentBranches.Select(x => x.UpStream).Cast<DMSTypeGraphNode>())
            {
                DMSTypeGraphNode currentNode = parent;

                while (currentNode.ParentBranches.Count != 0)
                {
                    currentNode = currentNode.ParentBranches.First().UpStream as DMSTypeGraphNode;
                }

                if (currentNode.DMSType != DMSType.ENERGYSOURCE)
                {
                    return false;
                }
            }

            return true;
        }

        private void EnqueueChildren(DMSTypeGraphNode currentNode, Queue<DMSTypeGraphNode> nodesToProcess)
        {
            var children = currentNode.ChildBranches.Select(x => x.DownStream).Cast<DMSTypeGraphNode>();

            foreach (var child in children)
            {
                nodesToProcess.Enqueue(child);
            }
        }

        /// <summary>
        /// Determines if the current node should be processed. Only breaker nodes with multiple parents should be processed.
        /// </summary>
        private bool ShouldNodeBeProcessed(DMSTypeGraphNode currentNode)
        {
            return IsNodeBreaker(currentNode) && DoesNodeHaveMultipleParents(currentNode);
        }

        private bool DoesNodeHaveMultipleParents(DMSTypeGraphNode node)
        {
            return node.ParentBranches.Count > 1;
        }

        private bool IsNodeBreaker(DMSTypeGraphNode currentNode)
        {
            return currentNode.DMSType == DMSType.BREAKER;
        }
    }
}
