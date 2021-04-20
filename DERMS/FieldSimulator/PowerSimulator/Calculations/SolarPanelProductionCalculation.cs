using FieldSimulator.PowerSimulator.Storage;

namespace FieldSimulator.PowerSimulator.Calculations
{
    class SolarPanelProductionCalculation : Calculation
    {
        private float nominalPower;

        public SolarPanelProductionCalculation(float nominalPower)
        {
            this.nominalPower = nominalPower;
        }

        public override void Calculate(PowerGridSimulatorStorage powerGridSimulatorStorage, double simulationInterval)
        {
            CalculationParameter outputPower = outputs[0];

            if (!powerGridSimulatorStorage.WeatherStorage.IsSunny)
            {
                powerGridSimulatorStorage.UpdateValue(outputPower.RemotePointType, outputPower.Address, 0);
                return;
            }

            float solarInsolation = 990 * (1 - 3 * powerGridSimulatorStorage.WeatherStorage.CloudCover / 100);
            float cellTemperature = powerGridSimulatorStorage.WeatherStorage.Temperature + 0.025f * solarInsolation;
            float generatedPower = nominalPower * solarInsolation * 0.00095f * (0.005f * (cellTemperature - 25) - 1);

            powerGridSimulatorStorage.UpdateValue(outputPower.RemotePointType, outputPower.Address, generatedPower);
        }
    }
}
