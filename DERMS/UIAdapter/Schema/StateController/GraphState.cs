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

        public Dictionary<long, EquipmentState> GetEquipmentStates()
        {
            return equipmentStates;
        }

        public long SourceGid { get { return graph.GetRoot().GlobalId; } }

        public SchemaBreakerGraphNode GetInterConnectedBreaker()
        {
            return graph.GetInterConnectedBreaker() as SchemaBreakerGraphNode;
        }

        public void PerformEnergizing(long breakerGid, NodeStateChangePropagator nodeStatePropagator, IInterConnectedBreakerState interConnectedBreakerState)
        {
            SchemaBreakerGraphNode interConnectedBreaker = graph.GetInterConnectedBreaker() as SchemaBreakerGraphNode;

            ProcessInterConnectedBreaker(interConnectedBreaker, nodeStatePropagator, interConnectedBreakerState);

            SchemaBreakerGraphNode breaker = graph.GetNode(breakerGid) as SchemaBreakerGraphNode;

            if (breaker == null || interConnectedBreaker.GlobalId == breaker.GlobalId)
            {
                return;
            }

            nodeStatePropagator.PropagateChanges(breaker, equipmentStates);

            ProcessInterConnectedBreaker(interConnectedBreaker, nodeStatePropagator, interConnectedBreakerState);
        }

        public void ChangeBreakerState(long breakerGid, BreakerState newBreakerState)
        {
            SchemaBreakerGraphNode breaker = graph.GetNode(breakerGid) as SchemaBreakerGraphNode;

            if (breaker == null)
            {
                return;
            }

            breaker.ChangeBreakerState(newBreakerState);
            BreakerEquipmentState breakerEquipmentState = equipmentStates[breaker.GlobalId] as BreakerEquipmentState;
            breakerEquipmentState.DoesConduct = breaker.Conduts;
            breakerEquipmentState.Closed = breaker.Conduts;
        }

        public SchemaGraphNode GetRoot()
        {
            return graph.GetRoot();
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
                BreakerEquipmentState breakerEquipmentState = equipmentStates[breaker.GlobalId] as BreakerEquipmentState;
                breakerEquipmentState.Closed = breaker.Conduts;
            }
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

            InitializeEquipmentStatesFromInterConnectedBreaker(nodeStatePropagator);
        }

        private void InitializeEquipmentStatesFromInterConnectedBreaker(NodeStateChangePropagator nodeStatePropagator)
        {
            SchemaBreakerGraphNode interConnectedBreaker = graph.GetInterConnectedBreaker() as SchemaBreakerGraphNode;

            if (interConnectedBreaker == null)
            {
                return;
            }

            SchemaInterConnectivityNodeGraphNode specialNode = interConnectedBreaker.ParentBranch.UpStream as SchemaInterConnectivityNodeGraphNode;
            specialNode.ChangeState(interConnectedBreaker.Conduts);

            nodeStatePropagator.PropagateChanges(specialNode, equipmentStates);
        }

        private void ProcessInterConnectedBreaker(SchemaBreakerGraphNode interConnectedBreaker, NodeStateChangePropagator nodeStatePropagator, IInterConnectedBreakerState interConnectedBreakerState)
        {
            bool interConnectedBreakerConducts;

            SchemaInterConnectivityNodeGraphNode specialConnectivityNode = interConnectedBreaker.ParentBranch.UpStream as SchemaInterConnectivityNodeGraphNode;
            if (interConnectedBreakerState.DoesInterConnectedBreakerConduct(SourceGid))
            {
                interConnectedBreakerConducts = true;
            }
            else
            {
                interConnectedBreakerConducts = interConnectedBreaker.DoesConduct;
            }

            specialConnectivityNode.ChangeState(interConnectedBreakerConducts);

            nodeStatePropagator.PropagateChanges(specialConnectivityNode, equipmentStates);
        }
    }
}
