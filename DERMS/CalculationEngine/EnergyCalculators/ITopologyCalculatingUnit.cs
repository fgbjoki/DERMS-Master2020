using System.Collections.Generic;

namespace CalculationEngine.EnergyCalculators
{
    public interface ITopologyCalculatingUnit
    {
        float Calculate(long sourceGid, IEnumerable<long> connectedNodesGids);
        float Recalculate(float calculatedValue, long modifiedEntity, float newValue);
    }
}