using System.Collections.Generic;
using System.Collections;
using FieldSimulator.PowerSimulator.Model.Graph.Graphs.Nodes;
using FieldSimulator.PowerSimulator.Model.Graph.Graphs.Branches;

namespace FieldSimulator.PowerSimulator.Model.Graph.GraphTraverser
{
    public class TopologyGraphTraverser : IEnumerable<TopologyGraphNode>
    {
        private TopologyGraphNode root;

        public TopologyGraphTraverser()
        {

        }

        public IEnumerator<TopologyGraphNode> GetEnumerator()
        {
            return new TopologyGraphBranchTraverser(root);
        }

        public void LoadRoot(TopologyGraphNode root)
        {
            this.root = root;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new TopologyGraphBranchTraverser(root);
        }
    }

    internal class TopologyGraphBranchTraverser : IEnumerator<TopologyGraphNode>
    {
        private Queue<TopologyGraphNode> nodesToProcess;
        private TopologyGraphNode currentNode;
        private TopologyGraphNode root;

        public TopologyGraphBranchTraverser(TopologyGraphNode root)
        {
            this.root = root;

            nodesToProcess = new Queue<TopologyGraphNode>();
            nodesToProcess.Enqueue(root);
        }

        public TopologyGraphNode Current
        {
            get
            {
                return currentNode;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return currentNode;
            }
        }

        public void Dispose()
        {
            nodesToProcess.Clear();
            nodesToProcess = null;
        }

        public bool MoveNext()
        {
            if (nodesToProcess.Count == 0)
            {
                return false;
            }

            currentNode = nodesToProcess.Dequeue();

            foreach (var childBranch in currentNode.ChildBranches)
            {
                TopologyGraphBranch topologyBranch = childBranch as TopologyGraphBranch;

                if (topologyBranch.DoesBranchConduct())
                {
                    nodesToProcess.Enqueue(topologyBranch.DownStream as TopologyGraphNode);
                }
            }

            return true;
        }

        public void Reset()
        {
            nodesToProcess.Clear();
            nodesToProcess.Enqueue(root);
        }
    }
}