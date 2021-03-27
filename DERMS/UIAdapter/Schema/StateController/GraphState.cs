using System.Collections.Generic;
using UIAdapter.Helpers;
using UIAdapter.Schema.Graph;
using System.Linq;
using Common.ComponentStorage;
using UIAdapter.Model.Schema;
using Common.Logger;

namespace UIAdapter.Schema.StateController
{
    public class GraphState
    {
        private SchemaGraph graph;

        private Dictionary<long, EquipmentState> equipmentStates;

        public GraphState(SchemaGraph graph)
        {
            this.graph = graph;

            equipmentStates = new Dictionary<long, EquipmentState>();
        }

        public void InitializeState(EquipmentStateCreator stateCreator, IStorage<Breaker> breakerStorage, BreakerMessageMapping breakerMessageMapping, NodeStateChangePropagator nodeStatePropagator)
        {
            equipmentStates = stateCreator.CreateEquipmentState(graph);

            LoadBreakerStates(graph.GetBreakers(), breakerStorage, breakerMessageMapping);
            InitializeEquipmentStates(nodeStatePropagator);
        }

        private void LoadBreakerStates(List<SchemaBreakerGraphNode> breakers, IStorage<Breaker> breakerStorage, BreakerMessageMapping breakerMessageMapping)
        {
            foreach (var breaker in breakers)
            {
                Breaker breakerState = breakerStorage.GetEntity(breaker.GlobalId);

                if (breakerState == null)
                {
                    Logger.Instance.Log($"[{GetType()}] Cannot find breaker state in storage... Breaker with gid: {breaker.GlobalId:X16} will have unknown state.");
                    return;
                }

                breaker.ChangeBreakerState(breakerMessageMapping.MapRawDataToBreakerState(breakerState.CurrentValue));
            }
        }

        public Dictionary<long, EquipmentState> GetEquipmentStates()
        {
            return equipmentStates;
        }

        public long SourceGid { get { return graph.GetRoot().GlobalId; } }

        public long GetInterConnectedBreakerGid()
        {
            SchemaGraphNode interConnectedBreaker = graph.GetInterConnectedBreaker();

            return interConnectedBreaker == null ? 0 : interConnectedBreaker.GlobalId;
        }

        public void BreakerStateChanged(long breakerGid, BreakerState newBreakerState, NodeStateChangePropagator nodeStatePropagator)
        {
            SchemaBreakerGraphNode breaker = graph.GetNode(breakerGid) as SchemaBreakerGraphNode;

            if (breaker == null)
            {
                return;
            }

            breaker.ChangeBreakerState(newBreakerState);

            nodeStatePropagator.PropagateChanges(breaker, equipmentStates);
        }

        public SchemaGraphNode GetRoot()
        {
            return graph.GetRoot();
        }

        private void EnqueueChildren(SchemaGraphNode node, Queue<SchemaGraphNode> nodeToProcess)
        {
            foreach (var child in node.ChildBranches.Select(x => x.DownStream))
            {
                nodeToProcess.Enqueue(child);
            }
        }

        private void InitializeEquipmentStates(NodeStateChangePropagator nodeStatePropagator)
        {
            nodeStatePropagator.PropagateChanges(graph.GetRoot(), equipmentStates);
        }

        private void PropagateParentState(SchemaGraphNode parent, SchemaGraphNode child)
        {
            if (parent != null)
            {
                child.IsEnergized = parent.DoesConduct;

                EquipmentState equipmentState = equipmentStates[child.GlobalId];
                equipmentState.IsEnergized = child.IsEnergized;
                equipmentState.DoesConduct = child.DoesConduct;
            }
        }
    }
}
