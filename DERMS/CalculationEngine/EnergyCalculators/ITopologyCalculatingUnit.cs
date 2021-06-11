using System.Collections.Generic;

namespace CalculationEngine.EnergyCalculators
{
    public interface ITopologyCalculatingUnit
    {
        float Calculate(EnergyBalanceCalculation energyBalanceCalculation, IEnumerable<long> connectedNodesGids);
        void Recalculate(EnergyBalanceCalculation energyBalance, long conductingEquipmentGid, float delta);
    }
}