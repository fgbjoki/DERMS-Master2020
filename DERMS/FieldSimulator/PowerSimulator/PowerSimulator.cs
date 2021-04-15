using FieldSimulator.PowerSimulator.SchemaLoader;
using CIM.Model;
using FieldSimulator.PowerSimulator.Model;
using FieldSimulator.PowerSimulator.Model.Graph;
using FieldSimulator.Modbus.SchemaAligner;
using FieldSimulator.PowerSimulator.Storage;
using Common.AbstractModel;
using System.Linq;
using FieldSimulator.PowerSimulator.Model.Measurements;

namespace FieldSimulator.PowerSimulator
{
    public class PowerSimulator : IPowerSimulator
    {
        private ISchemaLoader schemaLoader;

        private PowerGridGraphSimulator graphSimulator;

        private IRemotePointSchemaModelAligner schemaAligner;

        private RemotePointValueChangedPublisher valuePublisher;

        private PowerGridSimulatorStorage powerGridSimulatorStorage;

        public PowerSimulator(IRemotePointSchemaModelAligner schemaAligner, PowerGridSimulatorStorage powerGridSimulatorStorage, RemotePointValueChangedPublisher valuePublisher)
        {
            this.schemaAligner = schemaAligner;
            this.valuePublisher = valuePublisher;
            this.powerGridSimulatorStorage = powerGridSimulatorStorage;

            schemaLoader = new SchemaCIMLoader(new ModelCreator());

            graphSimulator = new PowerGridGraphSimulator(powerGridSimulatorStorage);
        }

        public ConcreteModel LoadSchema(string xmlFilePath)
        {
            return schemaLoader.LoadSchema(xmlFilePath);
        }

        public void Start()
        {
            // TODO
        }

        public void Stop()
        {
            // TODO
        }

        public EntityStorage CreateSlaveModel(ConcreteModel concreteModel)
        {
            return schemaLoader.CreateSlaveModel(concreteModel);
        }

        public void LoadModel(EntityStorage slaveModel)
        {
            powerGridSimulatorStorage.Clear();

            graphSimulator.CreateGraphs(slaveModel);
            schemaAligner.AlignRemotePoints(slaveModel);

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
    }
}
