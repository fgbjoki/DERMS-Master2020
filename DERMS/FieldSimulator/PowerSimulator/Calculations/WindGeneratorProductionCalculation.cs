using FieldSimulator.PowerSimulator.Storage;

namespace FieldSimulator.PowerSimulator.Calculations
{
    /// Input parameter indexes:
    /// 
    /// Output parameter indexes:
    /// 0 - ActivePower
    class WindGeneratorProductionCalculation : Calculation
    {
        private float startUpSpeed;
        private float cutOutSpeed;
        private float nominalSpeed;

        private float nominalPower;

        public WindGeneratorProductionCalculation(float startUpSpeed, float cutOutSpeed, float nominalSpeed, float nominalPower)
        {
            this.startUpSpeed = startUpSpeed;
            this.cutOutSpeed = cutOutSpeed;
            this.nominalSpeed = nominalSpeed;
            this.nominalPower = nominalPower;
        }

        public override void Calculate(PowerGridSimulatorStorage powerGridSimulatorStorage, double simulationInterval)
        {
            CalculationParameter activePowerParameter = outputs[0];

            if (!ShouldGeneratorWork(powerGridSimulatorStorage.WeatherStorage.WindKPH))
            {
                powerGridSimulatorStorage.UpdateValue(activePowerParameter.RemotePointType, activePowerParameter.Address, 0f);
                return;
            }

            float generatedPower = CalculateGeneratedPower(powerGridSimulatorStorage.WeatherStorage.WindKPH);

            powerGridSimulatorStorage.UpdateValue(activePowerParameter.RemotePointType, activePowerParameter.Address, generatedPower);
        }

        private float CalculateGeneratedPower(float windSpeed)
        {
            if (windSpeed >= startUpSpeed && windSpeed < nominalSpeed)
            {
                return (windSpeed - startUpSpeed) * 0.035f;
            }
            else
            {
                return nominalPower;
            }
        }

        private bool ShouldGeneratorWork(float windSpeed)
        {
            if (windSpeed < startUpSpeed || windSpeed >= cutOutSpeed)
            {
                return false;
            }

            return true;
        }
    }
}
