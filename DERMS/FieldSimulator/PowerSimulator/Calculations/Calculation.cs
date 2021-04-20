using FieldSimulator.Model;
using FieldSimulator.PowerSimulator.Storage;
using System.Collections.Generic;

namespace FieldSimulator.PowerSimulator.Calculations
{
    public struct CalculationParameter
    {
        public RemotePointType RemotePointType { get; set; }
        public ushort Address { get; set; }
    }

    public abstract class Calculation
    {
        protected List<CalculationParameter> inputs;
        protected List<CalculationParameter> outputs;

        protected Calculation()
        {
            inputs = new List<CalculationParameter>();
            outputs = new List<CalculationParameter>();
        }

        public void AddInputParameter(RemotePointType remotePointType, ushort address)
        {
            CalculationParameter newParameter = new CalculationParameter()
            {
                RemotePointType = remotePointType,
                Address = address
            };

            inputs.Add(newParameter);
        }

        public void AddOutput(RemotePointType remotePointType, ushort address)
        {
            CalculationParameter newParameter = new CalculationParameter()
            {
                RemotePointType = remotePointType,
                Address = address
            };

            outputs.Add(newParameter);
        }

        public abstract void Calculate(PowerGridSimulatorStorage powerGridSimulatorStorage, double simulationInterval);
    }
}
