namespace UIAdapter.Schema.Graph
{
    public class SchemaInterConnectivityNodeGraphNode : SchemaGraphNode
    {
        private bool breakerConducts;

        public SchemaInterConnectivityNodeGraphNode(long globalId) : base(globalId)
        {
        }

        public override bool DoesConduct
        {
            get
            {
                return base.DoesConduct || breakerConducts;
            }
        }

        public void ChangeState(bool interConnectedBreakerConducts)
        {
            breakerConducts = interConnectedBreakerConducts;
        }
    }
}
