using Common.PubSub.Messages;

namespace UIAdapter.Schema
{
    public interface IGraphSchemaController
    {
        void ProcessDiscreteValueChanges(long discreteGid, int value);

        void ProcessEnergyBalanceChange(EnergyBalanceChanged energyBalance);
    }
}