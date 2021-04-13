using FieldSimulator.PowerSimulator.Model.Graph.GraphManipulators;
using FieldSimulator.PowerSimulator.Model.Graph.Graphs;
using System.Collections.Generic;

namespace FieldSimulator.PowerSimulator.Model.Graph.ConnectivityGraphCreator.GraphRules
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
