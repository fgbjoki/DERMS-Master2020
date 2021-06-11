using CalculationEngine.Forecast.WeatherForecast.DataConverter.Regression.Functions;
using Common.DataTransferObjects;
using Common.WeatherAPI;
using System;
using System.Collections.Generic;
using System.Windows;

namespace CalculationEngine.Forecast.WeatherForecast.DataConverter.Regression
{
    public class InterpolationValueCreator
    {
        private Random random;

        public InterpolationValueCreator()
        {
            random = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
        }

        public RegressionFunctionCreator RegressionFunctionCreator { get; set; }

        public List<WeatherDataInfo> CreateInterpolatedValues(WeatherHourData firstPoint, WeatherHourData secondPoint)
        {
            return InterpolateWeatherData(firstPoint, secondPoint);
        }

        private List<WeatherDataInfo> InterpolateWeatherData(WeatherHourData firstPoint, WeatherHourData secondPoint)
        {
            Point[] samplePoints = new Point[3];
            List<WeatherDataInfo> interpolatedValues = new List<WeatherDataInfo>(58);

            List<float> windSpeed = InterpolateProperty(firstPoint.WindMPS, secondPoint.WindMPS, samplePoints);
            List<float> cloudCover = InterpolateProperty(firstPoint.CloudCover, secondPoint.CloudCover, samplePoints);
            List<float> temperatures = InterpolateProperty(firstPoint.Temperature, secondPoint.Temperature, samplePoints);

            for (int i = 0; i < windSpeed.Count; i++)
            {
                WeatherDataInfo newData = new WeatherDataInfo()
                {
                    WindMPS = windSpeed[i],
                    CloudCover = cloudCover[i],
                    TemperatureC = temperatures[i],
                };

                interpolatedValues.Add(newData);
            }

            return interpolatedValues;
        }

        // X -> time
        // Y -> property value
        private List<float> InterpolateProperty(float firstPoint, float secondPoint, Point[] samplePoints)
        {
            List<float> interpolatedValues = new List<float>(58);

            float midValue = GenerateRandomValueInRange(firstPoint, secondPoint);

            samplePoints[0].X = 0;
            samplePoints[0].Y = firstPoint;

            samplePoints[1].X = 30;
            samplePoints[1].Y = GenerateRandomValueInRange(firstPoint, secondPoint);

            samplePoints[2].X = 60;
            samplePoints[2].Y = secondPoint;

            ReggressionFunction function = RegressionFunctionCreator.CreateFunction(samplePoints);

            for (float i = 1; i <= 59; i++)
            {
                interpolatedValues.Add(function.Calculate(i));
            }

            return interpolatedValues;
        }

        private float GenerateRandomValueInRange(float firstValue, float secondValue)
        {
            float midValue = (firstValue + secondValue) / 2;

            float deviation = random.Next() % 2 == 0 ? midValue * 0.05f : -midValue * 0.05f;

            return midValue + deviation;
        }
    }
}
