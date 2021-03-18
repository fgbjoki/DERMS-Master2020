using System.Collections.Generic;

namespace CalculationEngine.Graphs
{
    public interface IGraphCreator<DependentType, DependentUponType>
    {
        IEnumerable<DependentType> CreateGraph(DependentUponType graph);
    }
}
