using Common.ComponentStorage;
using Common.DataTransferObjects.CalculationEngine;
using Common.Logger;
using Common.ServiceInterfaces.CalculationEngine;

namespace CalculationEngine.Commanding.DERCommanding.CommandValidation
{
    public abstract class BaseCommandingValidator<T> : IDERUnitCommandValidator
        where T : Model.DERCommanding.DistributedEnergyResource
    {
        private IDERStateDeterminator derStateDeterminator;
        private IStorage<Model.DERCommanding.DistributedEnergyResource> storage;

        protected BaseCommandingValidator(IDERStateDeterminator derStateDeterminator, IStorage<Model.DERCommanding.DistributedEnergyResource> DERStorage)
        {
            this.derStateDeterminator = derStateDeterminator;
            storage = DERStorage;
        }

        public CommandFeedback ValidateCommand(long derGid, float commandingValue)
        {
            T commandedEntity = GetEntity(derGid);
            if (commandedEntity == null)
            {
                Logger.Instance.Log($"[{GetType().Name}] Cannot find energy storage with gid: 0x:{derGid:X16}. Cannot validate command.");
                return new CommandFeedback()
                {
                    Successful = false,
                    Message = "Entity couldn't be found."
                };
            }

            return ValidateCommand(commandedEntity, commandingValue);
        }

        protected abstract CommandFeedback ValidateCommand(T derUnit, float commandingValue);

        private T GetEntity(long entityGid)
        {
            return storage.GetEntity(entityGid) as T;
        }
    }
}
