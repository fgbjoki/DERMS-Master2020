using System.Collections.Generic;

namespace CalculationEngine.EnergyCalculators
{
    public interface ITopologyCalculatingUnit
    {
        float Calculate(long sourceGid, IEnumerable<long> connectedNodesGids);
        void Recalculate(EnergyBalanceCalculation energyBalance, float delta);
    }
}