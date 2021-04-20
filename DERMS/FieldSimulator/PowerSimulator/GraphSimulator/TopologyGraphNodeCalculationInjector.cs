using FieldSimulator.PowerSimulator.Calculations;
using FieldSimulator.PowerSimulator.Model.Equipment;
using FieldSimulator.PowerSimulator.Model.Graph.Graphs.Nodes;

namespace FieldSimulator.PowerSimulator.GraphSimulator
{
    class TopologyGraphNodeCalculationInjector
    {
        public void InjectCalculation(EntityStorage entityStorage, TopologyGraphNode node)
        {
            PopulateCalculation(entityStorage, node);

            foreach (var shunt in node.Shunts)
            {
                PopulateCalculation(entityStorage, shunt);
            }
        }

        private void PopulateCalculation(EntityStorage entityStorage, ICalculationNode calculationNode)
        {
            ConductingEquipment conductingEquipment = entityStorage.Storage[calculationNode.DMSType][calculationNode.GlobalId] as ConductingEquipment;

            if (conductingEquipment == null)
            {
                return;
            }

            Calculation calculation = conductingEquipment.CreateCalculation();

            calculationNode.Calculation = calculation;
        }
    }
}
