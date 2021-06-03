namespace CalculationEngine.Forecast.WeatherForecast.DataConverter.Regression.Functions
{
    public class LinearReggressionFunction : ReggressionFunction
    {
        public LinearReggressionFunction(float slope, float intercept)
        {
            Slope = slope;
            Intercept = intercept;
        }

        public float Slope { get; private set; }

        public float Intercept { get; private set; }

        public override float Calculate(float x)
        {
            return Slope * x + Intercept;
        }
    }
}
