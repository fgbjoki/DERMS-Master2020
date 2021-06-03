using CalculationEngine.Forecast.WeatherForecast.DataConverter.Regression;
using Common.WeatherAPI;
using System;
using System.Collections.Generic;

namespace CalculationEngine.Forecast.WeatherForecast.DataConverter
{
    public class WeatherDataConverter : IWeatherDataConverter
    {
        private RegressionFunctionCreator regressionFunctionCreator;
        private InterpolationValueCreator interpolationValueCreator;

        public WeatherDataConverter()
        {
            regressionFunctionCreator = new RegressionFunctionCreator();
            interpolationValueCreator = new InterpolationValueCreator();
            interpolationValueCreator.RegressionFunctionCreator = regressionFunctionCreator;
        }

        public List<WeatherDataInfo> CovertData(List<WeatherDayData> apiData)
        {
            List<WeatherDataInfo> convertedData = new List<WeatherDataInfo>();

            WeatherHourData previousHour = null;

            foreach (var dayData in apiData)
            {
                convertedData.AddRange(ProcessDayData(dayData, ref previousHour));
            }

            return convertedData;
        }

        private List<WeatherDataInfo> ProcessDayData(WeatherDayData dayData, ref WeatherHourData pastHour)
        {
            List<WeatherDataInfo> dataInfos = new List<WeatherDataInfo>();

            bool dayLight = false;

            foreach (var hour in dayData)
            {
                dataInfos.AddRange(ProcessHourData(hour, pastHour, dayData, ref dayLight));

                pastHour = hour;
            }

            return dataInfos;
        }

        private List<WeatherDataInfo> ProcessHourData(WeatherHourData currentHourData, WeatherHourData pastHour, WeatherDayData dayData, ref bool dayLight)
        {
            List<WeatherDataInfo> hoursData = new List<WeatherDataInfo>();

            DateTime currentTime = dayData.DayTime + TimeSpan.FromHours(Convert.ToDouble(currentHourData.Hour));
            WeatherDataInfo dataInfo = new WeatherDataInfo()
            {
                CloudCover = currentHourData.CloudCover,
                TemperatureC = currentHourData.Temperature,
                WindMPS = currentHourData.WindMPS,
                CurrentTime = currentTime
            };

            ChangeDayLight(dataInfo, dayData, ref dayLight);

            dataInfo.Daylight = dayLight;

            if (ShouldInterpolate(pastHour))
            {
                InterpolateMinuteValues(hoursData, currentHourData, pastHour, dayData, ref dayLight);
            }

            hoursData.Add(dataInfo);

            return hoursData;
        }

        private void InterpolateMinuteValues(List<WeatherDataInfo> hoursData, WeatherHourData currentHourData, WeatherHourData pastHour, WeatherDayData dayData, ref bool dayLight)
        {
            List<WeatherDataInfo> interpolatedDatas = interpolationValueCreator.CreateInterpolatedValues(pastHour, currentHourData);

            for (int i = 0; i < interpolatedDatas.Count; i++)
            {
                interpolatedDatas[i].CurrentTime = dayData.DayTime + TimeSpan.FromHours(pastHour.Hour) + TimeSpan.FromMinutes(i + 1);
                ChangeDayLight(interpolatedDatas[i], dayData, ref dayLight);

                interpolatedDatas[i].Daylight = dayLight;
            }

            hoursData.AddRange(interpolatedDatas);
        }

        private void ChangeDayLight(WeatherDataInfo dataInfo, WeatherDayData dayData, ref bool dayLight)
        {
            if (ShouldChangeDayLightState(dataInfo, dayData.Sunrise))
            {
                dayLight = true;
            }
            else if (ShouldChangeDayLightState(dataInfo, dayData.Sunset))
            {
                dayLight = false;
            }
        }

        private bool ShouldChangeDayLightState(WeatherDataInfo dataInfo, TimeSpan timespan)
        {
            return dataInfo.CurrentTime.Hour == timespan.Hours && dataInfo.CurrentTime.Minute == timespan.Minutes;
        }

        private bool ShouldInterpolate(WeatherHourData pastHour)
        {
            return pastHour != null;
        }
    }
}