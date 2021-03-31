using System.Collections.Generic;

namespace CalculationEngine.Model.Topology.Graph
{
    public class GraphNode
    {        
        public GraphNode()
        {
            ParentBranches = new List<GraphBranch<GraphNode>>();
            ChildBranches = new List<GraphBranch<GraphNode>>();
        }

        public List<GraphBranch<GraphNode>> ParentBranches { get; set; }

        public List<GraphBranch<GraphNode>> ChildBranches { get; set; }
    }
}
