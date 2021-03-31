using Common.UIDataTransferObject.Schema;
using System.Collections.Generic;
using UIAdapter.Schema.Graph;
using UIAdapter.Schema.StateController;

namespace UIAdapter.Schema
{
    public class SchemaGraphToDTOConverter
    {
        public SubSchemaDTO ConvertGraph(GraphState graph)
        {
            SubSchemaConductingEquipmentEnergized equipmentStates = ConvertGraphState(graph.GetEquipmentStates());
            Dictionary<long, List<long>> parentChildRelations = CreateParentToChildRelation(graph.GetRoot());

            SubSchemaDTO dto = new SubSchemaDTO();
            dto.ConductingEquipmentStates = equipmentStates;
            dto.EnergySourceGid = graph.GetRoot().GlobalId;
            dto.InterConnectedBreaker = graph.GetInterConnectedBreakerGid();
            dto.ParentChildRelation = parentChildRelations;

            return dto;
        }

        public SubSchemaConductingEquipmentEnergized ConvertGraphState(Dictionary<long, EquipmentState> equipmentStates)
        {
            SubSchemaConductingEquipmentEnergized dto = new SubSchemaConductingEquipmentEnergized();
            dto.Nodes = new Dictionary<long, SubSchemaNodeDTO>(equipmentStates.Count);

            foreach (var equipmentState in equipmentStates)
            {
                SubSchemaNodeDTO nodeDto = new SubSchemaNodeDTO()
                {
                    DoesConduct = equipmentState.Value.DoesConduct,
                    IsEnergized = equipmentState.Value.IsEnergized,
                    GlobalId = equipmentState.Value.GlobalId
                };

                dto.Nodes.Add(nodeDto.GlobalId, nodeDto);
            }

            return dto;
        }

        public Dictionary<long, List<long>> CreateParentToChildRelation(SchemaGraphNode root)
        {
            Dictionary<long, List<long>> relations = new Dictionary<long, List<long>>();
            Queue<SchemaGraphNode> nodesToProcess = new Queue<SchemaGraphNode>();
            nodesToProcess.Enqueue(root);

            while (nodesToProcess.Count > 0)
            {
                SchemaGraphNode currentNode = nodesToProcess.Dequeue();

                List<long> children = new List<long>(currentNode.ChildBranches.Count);

                foreach (var childBranch in currentNode.ChildBranches)
                {
                    SchemaGraphNode childNode = childBranch.DownStream;

                    nodesToProcess.Enqueue(childNode);
                    children.Add(childNode.GlobalId);
                }

                relations.Add(currentNode.GlobalId, children);
            }

            return relations;
        }
    }
}
