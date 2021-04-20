using Common.AbstractModel;
using FieldSimulator.PowerSimulator.Storage;

namespace FieldSimulator.PowerSimulator.Calculations
{
    interface ICalculationNode
    {
        void Calculate(PowerGridSimulatorStorage powerGridSimulatorStorage, double simulationInterval);

        Calculation Calculation { get; set; }

        DMSType DMSType { get; }

        long GlobalId { get; }
    }
}
