using System.Collections.Generic;

namespace FieldSimulator.PowerSimulator.Model.Graph.TopologyGraphCreator
{
    public interface IGraphCreator<DependentType, DependentUponType>
    {
        IEnumerable<DependentType> CreateGraph(DependentUponType graph);
    }
}
