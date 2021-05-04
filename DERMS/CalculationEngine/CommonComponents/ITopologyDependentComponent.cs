using System;
using System.Collections.Generic;

namespace CalculationEngine.CommonComponents
{
    public interface ITopologyDependentComponent
    {
        void ProcessTopologyChanges();
        void ProcessAnalogChanges(List<Tuple<long, float>> changes);
    }
}