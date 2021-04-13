using CIM.Model;
using FieldSimulator.PowerSimulator.SchemaLoader;

namespace FieldSimulator.PowerSimulator
{
    public interface IPowerSimulator : ISchemaLoader
    {
        void Start();
        void CreateModel(ConcreteModel concreteModel);
    }
}
