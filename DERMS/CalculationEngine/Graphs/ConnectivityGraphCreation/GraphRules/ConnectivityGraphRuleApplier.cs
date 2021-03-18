using CalculationEngine.Model.Topology.Graph;
using CalculationEngine.Model.Topology.Graph.Connectivity;
using System.Collections.Generic;

namespace CalculationEngine.Graphs.ConnectivityGraphCreation.GraphRules
{
    public class ConnectivityGraphRuleApplier : IGraphRule
    {
        private List<IGraphRule> rules;

        public ConnectivityGraphRuleApplier(GraphBranchManipulator graphBranchManipulator)
        {
            rules = new List<IGraphRule>(2)
            {
                new EnergySourceDirectionReverseGraphRule(graphBranchManipulator),
                new ReversePathToInterConnectedBreakerGraphRule(graphBranchManipulator)
            };
        }

        public void ApplyRule(ConnectivityGraph graph)
        {
            foreach (var rule in rules)
            {
                rule.ApplyRule(graph);
            }
        }
    }
}
