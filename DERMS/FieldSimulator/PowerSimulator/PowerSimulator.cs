using FieldSimulator.PowerSimulator.SchemaLoader;
using CIM.Model;
using FieldSimulator.PowerSimulator.Model;
using FieldSimulator.PowerSimulator.Model.Graph;
using FieldSimulator.Modbus.SchemaAligner;

namespace FieldSimulator.PowerSimulator
{
    public class PowerSimulator : IPowerSimulator
    {
        private ISchemaLoader schemaLoader;

        private PowerGridGraphSimulator graphSimulator;

        private IRemotePointSchemaModelAligner schemaAligner;

        public PowerSimulator(IRemotePointSchemaModelAligner schemaAligner)
        {
            this.schemaAligner = schemaAligner;

            schemaLoader = new SchemaCIMLoader(new ModelCreator());

            graphSimulator = new PowerGridGraphSimulator();
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
            graphSimulator.CreateGraphs(slaveModel);
            schemaAligner.AlignRemotePoints(slaveModel);
        }
    }
}
