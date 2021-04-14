using FieldSimulator.PowerSimulator.SchemaLoader;
using CIM.Model;
using FieldSimulator.PowerSimulator.Model;
using System.Collections.Generic;
using Common.AbstractModel;
using System;
using FieldSimulator.PowerSimulator.Model.Graph;

namespace FieldSimulator.PowerSimulator
{
    public class PowerSimulator : IPowerSimulator
    {
        private ISchemaLoader schemaLoader;
        private ModelCreator modelCreator;

        private PowerGridGraphSimulator graphSimulator;

        public PowerSimulator()
        {
            schemaLoader = new SchemaCIMLoader();
            modelCreator = new ModelCreator();

            graphSimulator = new PowerGridGraphSimulator();
        }

        public void CreateModel(ConcreteModel concreteModel)
        {
            EntityStorage entityStorage = modelCreator.CreateModel(concreteModel);
            graphSimulator.CreateGraphs(entityStorage);
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
    }
}
