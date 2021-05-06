using Common.PubSub;
using System.Collections.Generic;
using Common.PubSub.Subscriptions;
using Common.ComponentStorage;
using UIAdapter.Model.Schema;
using Common.ServiceInterfaces.UIAdapter;
using Common.UIDataTransferObject.Schema;
using UIAdapter.PubSub.DynamicHandlers;

namespace UIAdapter.Schema
{
    public class SchemaRepresentation : ISubscriber, ISchema
    {
        private IStorage<EnergySource> energySourceStorage;
        private IStorage<Breaker> breakerStateStorage;

        private GraphSchemaController schemaController;

        private SchemaAligner schemaAligner;
        
        public SchemaRepresentation(Storage<EnergySource> energySourceStorage, IStorage<Breaker> breakerStateStorage)
        {
            this.breakerStateStorage = breakerStateStorage;

            this.energySourceStorage = energySourceStorage;

            schemaController = new GraphSchemaController(breakerStateStorage, new NodeStateChangePropagator());

            schemaAligner = new SchemaAligner(schemaController, energySourceStorage.Commited);
        }

        public IEnumerable<ISubscription> GetSubscriptions()
        {
            return new List<ISubscription>(1)
            {
                new Subscription(Topic.DiscreteRemotePointChange, new SchemaBreakerStateChangedDynamichandler(schemaController)),
                new Subscription(Topic.EnergyBalanceChange, new EnergyBalanceDynamicHandler(schemaController))
            };
        }

        public SubSchemaDTO GetSchema(long energySourceId)
        {
            return schemaController.GetSchema(energySourceId);
        }

        public SubSchemaConductingEquipmentEnergized GetEquipmentStates(long energySourceId)
        {
            return schemaController.GetEquipmentStates(energySourceId);
        }

        public SchemaEnergyBalanceDTO GetEnergyBalance(long energySourceId)
        {
            return schemaController.GetEnergyBalanceChange(energySourceId);
        }

        public List<EnergySourceDTO> GetSubstations()
        {
            List<EnergySource> energySources = energySourceStorage.GetAllEntities();
            List<EnergySourceDTO> substations = new List<EnergySourceDTO>(energySources.Count);

            foreach (var energySource in energySources)
            {
                EnergySourceDTO energySourceDTO = new EnergySourceDTO()
                {
                    GlobalId = energySource.GlobalId,
                    SubstationGid = energySource.SubstationGid,
                    SubstationName = energySource.SubstationName
                };

                substations.Add(energySourceDTO);
            }

            return substations;
        }

        public long SubStationContainsEntity(long entityGid)
        {
            return schemaController.GetSubstationForEntity(entityGid);
        }
    }
}
