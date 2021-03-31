namespace CalculationEngine.Model.Topology.Graph
{
    public abstract class BaseGraphBranchManipulator<GraphNodeType>
        where GraphNodeType : GraphNode
    {
        public GraphBranch<GraphNode> AddBranch(GraphNodeType parent, GraphNodeType child)
        {
            GraphBranch<GraphNode> newBranch = CreateNewBranch(parent, child);
            parent.ChildBranches.Add(newBranch);
            child.ParentBranches.Add(newBranch);

            return newBranch;
        }

        public void ReverseBranchDirection(GraphBranch<GraphNode> branch)
        {
            GraphNode upStream = branch.UpStream;
            GraphNode downStream = branch.DownStream;

            DeleteBranch(branch);

            branch.ReverseDirection();

            downStream.ChildBranches.Add(branch);
            upStream.ParentBranches.Add(branch);
        }

        public void DeleteBranch(GraphBranch<GraphNode> branch)
        {
            GraphNode upStream = branch.UpStream;
            GraphNode downStream = branch.DownStream;

            upStream.ChildBranches.Remove(branch);
            downStream.ParentBranches.Remove(branch);
        }

        protected abstract GraphBranch<GraphNode> CreateNewBranch(GraphNodeType parent, GraphNodeType child);
    }
}
