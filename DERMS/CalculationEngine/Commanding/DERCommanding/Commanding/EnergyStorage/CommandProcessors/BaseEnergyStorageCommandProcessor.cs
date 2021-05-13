using CalculationEngine.Commanding.Commands;

namespace CalculationEngine.Commanding.DERCommanding.Commanding.EnergyStorage.CommandProcessors
{
    public abstract class BaseEnergyStorageCommandProcessor
    {
        public abstract Command CreateCommand(Model.DERCommanding.EnergyStorage energyStorage, float activePower);
    }
}
