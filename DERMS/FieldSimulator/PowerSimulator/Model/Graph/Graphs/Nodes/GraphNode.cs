using Common.AbstractModel;
using FieldSimulator.PowerSimulator.Model.Graph.Graphs.Branches;
using System.Collections.Generic;

namespace FieldSimulator.PowerSimulator.Model.Graph.Graphs.Nodes
{
    public class GraphNode
    {        
        public GraphNode(long globalId)
        {
            GlobalId = globalId;

            DMSType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(globalId);

            ParentBranches = new List<GraphBranch<GraphNode>>();
            ChildBranches = new List<GraphBranch<GraphNode>>();
        }

        public long GlobalId { get; set; }

        public DMSType DMSType { get; private set; }

        public List<GraphBranch<GraphNode>> ParentBranches { get; set; }

        public List<GraphBranch<GraphNode>> ChildBranches { get; set; }
    }
}
