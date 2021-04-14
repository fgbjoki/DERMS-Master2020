using FieldSimulator.PowerSimulator.Model.Graph.GraphManipulators;
using FieldSimulator.PowerSimulator.Model.Graph.Graphs.Branches;
using FieldSimulator.PowerSimulator.Model.Graph.Graphs.Nodes;
using FieldSimulator.PowerSimulator.Model.Graph.TopologyGraphCreator.GraphRules;
using System.Collections.Generic;
using System.Linq;

namespace FieldSimulator.PowerSimulator.Model.Graph.TopologyGraphCreator
{
    public class TopologyGraphCreator : MultipleRootDependentGraphCreator<TopologyGraphNode, ConnectivityGraphNode>
    {
        private TopologyGraphBranchManipulator aclineSegmentBranchManipulator;
        private TopologyBreakerGraphBranchManipulator breakerBrachManipulator;

        private BreakerReductionGraphRule breakerReductionGraphRule;

        private InterConnectedConnectivityCorrector corrector;

        public TopologyGraphCreator(TopologyGraphBranchManipulator aclineSegmentBranchManipulator, TopologyBreakerGraphBranchManipulator breakerBrachManipulator) : base(aclineSegmentBranchManipulator)
        {
            this.aclineSegmentBranchManipulator = aclineSegmentBranchManipulator;
            this.breakerBrachManipulator = breakerBrachManipulator;
            breakerReductionGraphRule = new BreakerReductionGraphRule(breakerBrachManipulator);

            corrector = new InterConnectedConnectivityCorrector(breakerBrachManipulator);        
        }

        public IEnumerable<TopologyBreakerGraphBranch> GetBreakerBranches()
        {
            return breakerReductionGraphRule.GetBreakerBranches();
        }

        public override IEnumerable<IMultipleRootGraph<TopologyGraphNode>> CreateGraph(IMultipleRootGraph<ConnectivityGraphNode> graph)
        {
            IEnumerable<IMultipleRootGraph<TopologyGraphNode>> newGraphs = base.CreateGraph(graph);
            TopologyGraph newGraph = newGraphs.First() as TopologyGraph;

            newGraph.LoadBreakerBranches(GetBreakerBranches());
            newGraph.LoadBreakerBranches(corrector.GetBreakerBranches());

            return newGraphs;
        }

        protected override TopologyGraphNode CreateNewNode(ConnectivityGraphNode dependentNode)
        {
            return new TopologyGraphNode(dependentNode.GlobalId);
        }

        protected override IEnumerable<GraphReductionRule<TopologyGraphNode>> GetReductionRules()
        {
            return new List<GraphReductionRule<TopologyGraphNode>>()
            {
                new TopologyACLSBranchGraphRule(aclineSegmentBranchManipulator),
                breakerReductionGraphRule,
                new ConnectivityNodeShuntReductionGraphRule(aclineSegmentBranchManipulator)
            };
        }

        protected override IMultipleRootGraph<TopologyGraphNode> InstantiateNewGraph(IMultipleRootGraph<ConnectivityGraphNode> graph)
        {
            return new TopologyGraph();
        }

        protected override void AdditionalProcessing(IMultipleRootGraph<TopologyGraphNode> graph)
        {
            corrector.CorrectInterConnection(graph);
        }
    }
}
