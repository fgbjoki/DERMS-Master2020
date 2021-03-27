using Common.ComponentStorage;
using Common.DataTransferObjects.CalculationEngine;
using System.Collections.Generic;
using System.Threading;
using UIAdapter.Helpers;
using UIAdapter.Model.Schema;
using UIAdapter.Schema.Graph;
using UIAdapter.Schema.StateController;
using Common.UIDataTransferObject.Schema;

namespace UIAdapter.Schema
{
    public class GraphSchemaController
    {
        private ReaderWriterLockSlim locker;
        private Dictionary<long, GraphState> graphs;
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
            locker.EnterWriteLock();

            foreach (var graph in graphs)
            {
                graph.Value.BreakerStateChanged(discreteGid, breakerMessageMapping.MapRawDataToBreakerState(value), nodeStatePropagator);
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
    }
}
