using FieldSimulator.PowerSimulator.SchemaLoader;

namespace FieldSimulator.PowerSimulator
{
    public interface IPowerSimulator : ISchemaLoader
    {
        void Start();
        void Stop();
        void LoadModel(EntityStorage slaveModel);
    }
}
