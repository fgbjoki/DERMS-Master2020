using FieldSimulator.PowerSimulator.SchemaLoader;
using CIM.Model;
using FieldSimulator.PowerSimulator.Model;
using FieldSimulator.Modbus.SchemaAligner;
using FieldSimulator.PowerSimulator.Storage;
using Common.AbstractModel;
using System.Linq;
using FieldSimulator.PowerSimulator.Model.Measurements;
using FieldSimulator.PowerSimulator.GraphSimulator;
using FieldSimulator.PowerSimulator.Model.Graph.GraphTraverser;
using FieldSimulator.PowerSimulator.Model.Equipment;

namespace FieldSimulator.PowerSimulator
{
    public class PowerSimulator : IPowerSimulator
    {
        private ISchemaLoader schemaLoader;

        private PowerGridGraphSimulator graphSimulator;

        private IRemotePointSchemaModelAligner schemaAligner;

        private RemotePointValueChangedPublisher valuePublisher;

        private PowerGridSimulatorStorage powerGridSimulatorStorage;

        private BreakerTopologyManipulation breakerManipulator;

        public PowerSimulator(IRemotePointSchemaModelAligner schemaAligner, PowerGridSimulatorStorage powerGridSimulatorStorage, RemotePointValueChangedPublisher valuePublisher, BreakerTopologyManipulation breakerManipulator)
        {
            this.schemaAligner = schemaAligner;
            this.valuePublisher = valuePublisher;
            this.breakerManipulator = breakerManipulator;
            this.powerGridSimulatorStorage = powerGridSimulatorStorage;

            schemaLoader = new SchemaCIMLoader(new ModelCreator());

            graphSimulator = new PowerGridGraphSimulator(powerGridSimulatorStorage, 5);
        }

        public ConcreteModel LoadSchema(string xmlFilePath)
        {
            return schemaLoader.LoadSchema(xmlFilePath);
        }

        public void Start()
        {
            graphSimulator.Start();
        }

        public void Stop()
        {
            graphSimulator.Stop();
        }

        public EntityStorage CreateSlaveModel(ConcreteModel concreteModel)
        {
            return schemaLoader.CreateSlaveModel(concreteModel);
        }

        public void LoadModel(EntityStorage slaveModel)
        {
            powerGridSimulatorStorage.ReloadStorages();

            graphSimulator.CreateGraphs(slaveModel);
            schemaAligner.AlignRemotePoints(slaveModel);

            graphSimulator.LoadBreakerBranches(breakerManipulator);
            PopulateBreakerMapping(slaveModel);
            PopulateStorage(slaveModel);
        }

        private void PopulateStorage(EntityStorage slaveModel)
        {
            foreach (var analogMeasurement in slaveModel.Storage[DMSType.MEASUREMENTANALOG].Values.Cast<AnalogMeasurement>())
            {
                powerGridSimulatorStorage.AddItem(analogMeasurement.RemotePointType, analogMeasurement.Address, analogMeasurement.Value);
            }

            foreach (var discreteMeasurement in slaveModel.Storage[DMSType.MEASUREMENTDISCRETE].Values.Cast<DiscreteMeasurement>())
            {
                powerGridSimulatorStorage.AddItem(discreteMeasurement.RemotePointType, discreteMeasurement.Address, discreteMeasurement.Value);
            }
        }

        private void PopulateBreakerMapping(EntityStorage slaveModel)
        {
            foreach (var breaker in slaveModel.Storage[DMSType.BREAKER].Values.Cast<Breaker>())
            {
                breakerManipulator.AddBreakerMapping(breaker.Measurements[0].Address, breaker.GlobalId);
            }
        }
    }
}
