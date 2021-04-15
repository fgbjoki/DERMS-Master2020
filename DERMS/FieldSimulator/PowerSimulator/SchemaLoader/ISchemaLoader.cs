using CIM.Model;

namespace FieldSimulator.PowerSimulator.SchemaLoader
{
    public interface ISchemaLoader
    {
        ConcreteModel LoadSchema(string xmlFilePath);
        EntityStorage CreateSlaveModel(ConcreteModel concreteModel);
    }
}
