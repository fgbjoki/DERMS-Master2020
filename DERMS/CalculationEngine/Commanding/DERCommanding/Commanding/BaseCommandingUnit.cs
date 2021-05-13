using CalculationEngine.Commanding.Commands;
using CalculationEngine.Model.DERCommanding;
using Common.ComponentStorage;

namespace CalculationEngine.Commanding.DERCommanding.Commanding
{
    public abstract class BaseCommandingUnit<T> : IDERCommandCreator
        where T : DistributedEnergyResource
    {
        private IStorage<DistributedEnergyResource> storage;

        public BaseCommandingUnit(IStorage<DistributedEnergyResource> storage)
        {
            this.storage = storage;
        }

        public Command CreateCommand(long derGid, float commandingValue)
        {
            T derEntity = GetEntity(derGid);

            return CreateCommand(derEntity, commandingValue);
        }

        protected abstract Command CreateCommand(T derEntity, float commandingValue);

        protected T GetEntity(long derGid)
        {
            return storage.GetEntity(derGid) as T;
        }
    }
}
