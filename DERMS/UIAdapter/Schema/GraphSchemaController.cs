using Common.ComponentStorage;
using Common.DataTransferObjects.CalculationEngine;
using System.Collections.Generic;
using System.Threading;
using UIAdapter.Helpers;
using UIAdapter.Model.Schema;
using UIAdapter.Schema.Graph;
using UIAdapter.Schema.StateController;
using Common.UIDataTransferObject.Schema;
using Common.Logger;
using Common.PubSub.Messages;

namespace UIAdapter.Schema
{
    public class GraphSchemaController : IGraphSchemaController, IInterConnectedBreakerState
    {
        private ReaderWriterLockSlim locker;
        private Dictionary<long, GraphState> graphs;
        private Dictionary<long, SchemaEnergyBalance> energyBalances;
        private DynamicSchemaToGraphSchemaConverter ceToUIAdapterConverter;

        private EquipmentStateCreator stateCreator;

        private BreakerMessageMapping breakerMessageMapping;

        private NodeStateChangePropagator nodeStatePropagator;

        private SchemaGraphToDTOConverter dtoConverter;

        private IStorage<Breaker> breakerStateStorage;

        public GraphSchemaController(IStorage<Breaker> breakerStateStorage, NodeStateChangePropagator nodeStatePropagator)
        {
            this.breakerStateStorage = breakerStateStorage;
            this.nodeStatePropagator = nodeStatePropagator;

            breakerMessageMapping = new BreakerMessageMapping();

            locker = new ReaderWriterLockSlim();

            stateCreator = new EquipmentStateCreator();
            graphs = new Dictionary<long, GraphState>();
            energyBalances = new Dictionary<long, SchemaEnergyBalance>();
            ceToUIAdapterConverter = new DynamicSchemaToGraphSchemaConverter();
            dtoConverter = new SchemaGraphToDTOConverter();
        }

        public void AddNewSchema(SchemaGraphChanged newSchema)
        {
            SchemaGraph graph = ceToUIAdapterConverter.Convert(newSchema);

            locker.EnterWriteLock();

            GraphState newGraphState = new GraphState(graph);
            newGraphState.InitializeState(stateCreator, breakerStateStorage, breakerMessageMapping, nodeStatePropagator);

            graphs.Add(newGraphState.SourceGid, newGraphState);

            locker.ExitWriteLock();
        }

        public void ProcessDiscreteValueChanges(long discreteGid, int value)
        {
            Breaker breaker = breakerStateStorage.GetEntity(discreteGid);

            if (breaker == null)
            {
                Logger.Instance.Log($"[{GetType().Name}] Cannot find breaker with discrete gid {discreteGid:X16}. Schema may be in fault.");
                return;
            }

            locker.EnterWriteLock();

            foreach (var graph in graphs)
            {
                graph.Value.ChangeBreakerState(breaker.GlobalId, breakerMessageMapping.MapRawDataToBreakerState(value));
            }

            foreach (var graph in graphs)
            {
                graph.Value.PerformEnergizing(breaker.GlobalId, nodeStatePropagator, this);
            }

            locker.ExitWriteLock();
        }

        public List<long> GetSchemaSources()
        {
            List<long> schemaSources;

            locker.EnterReadLock();

            schemaSources = new List<long>(graphs.Keys);

            locker.ExitReadLock();

            return schemaSources;
        }

        public SubSchemaDTO GetSchema(long energySourceId)
        {
            SubSchemaDTO dto;

            locker.EnterReadLock();

            dto = dtoConverter.ConvertGraph(graphs[energySourceId]);

            locker.ExitReadLock();

            return dto;
        }

        public SubSchemaConductingEquipmentEnergized GetEquipmentStates(long energySourceId)
        {
            SubSchemaConductingEquipmentEnergized equipmentStatesDto;

            locker.EnterReadLock();

            equipmentStatesDto = dtoConverter.ConvertGraphState(graphs[energySourceId].GetEquipmentStates());

            locker.ExitReadLock();

            return equipmentStatesDto;
        }

        public bool DoesInterConnectedBreakerConduct(long energySourceGid)
        {
            foreach (var graph in graphs)
            {
                if (energySourceGid == graph.Value.SourceGid)
                {
                    continue;
                }

                return graph.Value.GetInterConnectedBreaker().DoesConduct;
            }

            return false;
        }

        public void ProcessEnergyBalanceChange(EnergyBalanceChanged energyBalance)
        {
            locker.EnterWriteLock();

            SchemaEnergyBalance currentSchemaBalance;
            if (!energyBalances.TryGetValue(energyBalance.EnergySourceGid, out currentSchemaBalance))
            {
                currentSchemaBalance = new SchemaEnergyBalance()
                {
                    EnergySourceGid = energyBalance.EnergySourceGid
                };

                energyBalances[energyBalance.EnergySourceGid] = currentSchemaBalance;
            }

            currentSchemaBalance.DemandEnergy = energyBalance.DemandEnergy;
            currentSchemaBalance.ImportedEnergy = energyBalance.ImportedEnergy;
            currentSchemaBalance.ProducedEnergy = energyBalance.ProducedEnergy;

            locker.ExitWriteLock();
        }

        public SchemaEnergyBalance GetEnergyBalanceChange(long energySourceGid)
        {
            locker.EnterReadLock();

            SchemaEnergyBalance currentSchemaBalance;
            energyBalances.TryGetValue(energySourceGid, out currentSchemaBalance);

            locker.ExitReadLock();

            return currentSchemaBalance;
        }
    }
}
