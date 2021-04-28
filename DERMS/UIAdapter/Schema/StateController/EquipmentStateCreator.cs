using System.Collections.Generic;
using System.Linq;
using UIAdapter.Schema.Graph;

namespace UIAdapter.Schema.StateController
{
    public class EquipmentStateCreator
    {
        public Dictionary<long, EquipmentState> CreateEquipmentState(SchemaGraph graph)
        {
            Dictionary<long, EquipmentState> equipmentStates = new Dictionary<long, EquipmentState>();

            SchemaGraphNode root = graph.GetRoot();
            Queue<SchemaGraphNode> nodesToProcess = new Queue<SchemaGraphNode>();
            nodesToProcess.Enqueue(root);

            while (nodesToProcess.Count > 0)
            {
                SchemaGraphNode currentNode = nodesToProcess.Dequeue();

                AddNewEquipmentState(equipmentStates, currentNode);

                EnqueueChildren(nodesToProcess, currentNode);
            }

            return equipmentStates;
        }

        private void AddNewEquipmentState(Dictionary<long, EquipmentState> equipmentStates, SchemaGraphNode currentNode)
        {
            EquipmentState newState = currentNode.GetEquipmentState();

            equipmentStates.Add(newState.GlobalId, newState);
        }

        private void EnqueueChildren(Queue<SchemaGraphNode> nodesToProcess, SchemaGraphNode currentNode)
        {
            foreach (var child in currentNode.ChildBranches.Select(x => x.DownStream))
            {
                nodesToProcess.Enqueue(child);
            }
        }
    }
}
