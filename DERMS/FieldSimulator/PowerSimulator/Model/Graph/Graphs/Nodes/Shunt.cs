using Common.AbstractModel;
using FieldSimulator.PowerSimulator.Calculations;
using FieldSimulator.PowerSimulator.Storage;

namespace FieldSimulator.PowerSimulator.Model.Graph.Graphs.Nodes
{
    public class Shunt : ICalculationNode
    {
        public Shunt(long globalId)
        {
            GlobalId = globalId;
            DMSType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(globalId);
        }

        public long GlobalId { get; private set; }

        public DMSType DMSType { get; private set; }

        public Calculation Calculation { get; set; }

        public void Calculate(PowerGridSimulatorStorage powerGridSimulatorStorage, double simulationInterval)
        {
            Calculation?.Calculate(powerGridSimulatorStorage, simulationInterval);
        }
    }
}
