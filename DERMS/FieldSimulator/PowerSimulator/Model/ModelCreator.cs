using CIM.Model;
using Common.AbstractModel;
using FieldSimulator.PowerSimulator.Model.Creators;
using FTN.ESI.SIMES.CIM.CIMAdapter.Importer;
using System.Collections.Generic;

namespace FieldSimulator.PowerSimulator.Model
{
    public class ModelCreator
    {
        private Dictionary<DMSType, IEntityCreator> entityCreators;
        private ImportHelper importHelper;

        public ModelCreator()
        {
            importHelper = new ImportHelper();
            InitializeEntityCreators();   
        }

        public EntityStorage CreateModel(ConcreteModel concreteModel)
        {
            EntityStorage entityStorage = new EntityStorage();

            entityCreators[DMSType.BREAKER].CreateNewEntities(concreteModel, entityStorage);
            entityCreators[DMSType.ACLINESEG].CreateNewEntities(concreteModel, entityStorage);
            entityCreators[DMSType.ENERGYSOURCE].CreateNewEntities(concreteModel, entityStorage);
            entityCreators[DMSType.ENERGYCONSUMER].CreateNewEntities(concreteModel, entityStorage);
            entityCreators[DMSType.ENERGYSTORAGE].CreateNewEntities(concreteModel, entityStorage);
            entityCreators[DMSType.SOLARGENERATOR].CreateNewEntities(concreteModel, entityStorage);
            entityCreators[DMSType.WINDGENERATOR].CreateNewEntities(concreteModel, entityStorage);

            entityCreators[DMSType.MEASUREMENTANALOG].CreateNewEntities(concreteModel, entityStorage);
            entityCreators[DMSType.MEASUREMENTDISCRETE].CreateNewEntities(concreteModel, entityStorage);

            entityCreators[DMSType.CONNECTIVITYNODE].CreateNewEntities(concreteModel, entityStorage);
            entityCreators[DMSType.TERMINAL].CreateNewEntities(concreteModel, entityStorage);

            return entityStorage;
        }

        private void InitializeEntityCreators()
        {
            entityCreators = new Dictionary<DMSType, IEntityCreator>();
            entityCreators[DMSType.BREAKER] = new BreakerCreator(importHelper);
            entityCreators[DMSType.TERMINAL] = new TerminalCreator(importHelper);
            entityCreators[DMSType.ACLINESEG] = new ACLineSegmentCreator(importHelper);
            entityCreators[DMSType.SOLARGENERATOR] = new SolarPanelCreator(importHelper);
            entityCreators[DMSType.ENERGYSOURCE] = new EnergySourceCreator(importHelper);
            entityCreators[DMSType.WINDGENERATOR] = new WindGeneratorCreator(importHelper);
            entityCreators[DMSType.ENERGYSTORAGE] = new EnergyStorageCreator(importHelper);
            entityCreators[DMSType.ENERGYCONSUMER] = new EnergyConsumerCreator(importHelper);
            entityCreators[DMSType.CONNECTIVITYNODE] = new ConnectivityNodeCreator(importHelper);
            entityCreators[DMSType.MEASUREMENTANALOG] = new AnalogMeasurementCreator(importHelper);
            entityCreators[DMSType.MEASUREMENTDISCRETE] = new DiscreteMeasurementCreator(importHelper);
        }
    }
}
