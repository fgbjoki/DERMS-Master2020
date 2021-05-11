using Common.WeatherAPI;
using System.Collections.Generic;
using System;

namespace FieldSimulator.PowerSimulator.Storage.Weather
{
    class WeatherDataContainer : IWeatherDataContainer
    {
        private List<WeatherDayData> weatherDayData;
        private int currentDay;
        private int currentHour;

        public WeatherDataContainer(List<WeatherDayData> weatherDayData, int currentHour)
        {
            this.weatherDayData = weatherDayData;
            this.currentHour = currentHour;

            PopulateData();
        }

        public float Temperature { get; set; }

        public bool IsSunny { get; set; }

        public float WindMPS { get; set; }

        public float CloudCover { get; set; }

        public StorageLock StorageLock
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void FetchNextHourData()
        {
            if (currentHour == 23)
            {
                currentHour = 0;
                currentDay++;
            }
            else
            {
                currentHour++;
            }

            PopulateData();
        }

        private void PopulateData()
        {
            Temperature = weatherDayData[currentDay][currentHour].Temperature;
            IsSunny = weatherDayData[currentDay][currentHour].IsSunny;
            WindMPS = weatherDayData[currentDay][currentHour].WindMPS;
            CloudCover = weatherDayData[currentDay][currentHour].CloudCover;
        }
    }
}
