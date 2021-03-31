using System.Collections.Generic;
using System.Linq;
using UIAdapter.Schema.Graph;
using UIAdapter.Schema.StateController;

namespace UIAdapter.Schema
{
    public class NodeStateChangePropagator
    {
        public void PropagateChanges(SchemaGraphNode propagationNode, Dictionary<long, EquipmentState> equipmentStates)
        {
            Queue<SchemaGraphNode> nodeToProcess = new Queue<SchemaGraphNode>();

            EnqueueChildren(propagationNode, nodeToProcess);

            while (nodeToProcess.Count > 0)
            {
                SchemaGraphNode currentNode = nodeToProcess.Dequeue();

                bool energized = currentNode.ParentBranch.UpStream.DoesConduct;

                ApplyChangesOnNode(currentNode, energized, nodeToProcess, equipmentStates);

                EnqueueChildren(currentNode, nodeToProcess);
            }
        }

        private SchemaGraphNode ApplyChangesOnNode(SchemaGraphNode node, bool energized, Queue<SchemaGraphNode> nodeToProcess, Dictionary<long, EquipmentState> equipmentStates)
        {
            node.IsEnergized = energized;

            EquipmentState equipmentState = equipmentStates[node.GlobalId];

            equipmentState.IsEnergized = energized;
            equipmentState.DoesConduct = node.DoesConduct;
            return node;
        }

        private void EnqueueChildren(SchemaGraphNode node, Queue<SchemaGraphNode> nodeToProcess)
        {
            foreach (var child in node.ChildBranches.Select(x => x.DownStream))
            {
                nodeToProcess.Enqueue(child);
            }
        }
    }
}
