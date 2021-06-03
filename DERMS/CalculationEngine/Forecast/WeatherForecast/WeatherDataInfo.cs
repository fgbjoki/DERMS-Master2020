using System;

namespace CalculationEngine.Forecast.WeatherForecast
{
    public class WeatherDataInfo
    {
        public DateTime CurrentTime { get; set; }

        public bool Daylight { get; set; }

        /// <summary>
        /// Wind speed [m/s]
        /// </summary>
        public float WindMPS { get; set; }

        public float CloudCover { get; set; }

        /// <summary>
        /// Temperature in Celsius
        /// </summary>
        public float TemperatureC { get; set; }
    }
}
