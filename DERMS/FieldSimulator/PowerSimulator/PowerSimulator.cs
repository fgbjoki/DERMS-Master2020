using FieldSimulator.PowerSimulator.SchemaLoader;
using CIM.Model;
using FieldSimulator.PowerSimulator.Model;
using System.Collections.Generic;
using Common.AbstractModel;

namespace FieldSimulator.PowerSimulator
{
    public class PowerSimulator : IPowerSimulator
    {
        private ISchemaLoader schemaLoader;
        private ModelCreator modelCreator;

        public PowerSimulator()
        {
            schemaLoader = new SchemaCIMLoader();
            modelCreator = new ModelCreator();
        }

        public void CreateModel(ConcreteModel concreteModel)
        {
            Dictionary<DMSType, Dictionary<long, IdentifiedObject>> newModel = modelCreator.CreateModel(concreteModel);
        }

        public ConcreteModel LoadSchema(string xmlFilePath)
        {
            return schemaLoader.LoadSchema(xmlFilePath);
        }

        public void Start()
        {
            // TODO
        }
    }
}
