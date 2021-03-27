namespace UIAdapter.Schema.Graph
{
    public class SchemaGraphBranch
    {
        public SchemaGraphBranch(SchemaGraphNode parent, SchemaGraphNode child)
        {
            UpStream = parent;
            DownStream = child;
        }

        public SchemaGraphNode UpStream { get; set; }
        public SchemaGraphNode DownStream { get; set; }
    }
}
