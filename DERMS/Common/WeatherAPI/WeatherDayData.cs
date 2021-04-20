using System;
using System.Collections;
using System.Collections.Generic;

namespace Common.WeatherAPI
{
    public class WeatherDayData : IEnumerable<WeatherHourData>
    {
        private WeatherHourData[] dataByHour = new WeatherHourData[24];
        private byte dataHourElements = 0;
        private DateTime date;

        public WeatherDayData(DateTime date)
        {
            this.date = date;
        }

        public void AddHourData(WeatherHourData hourData)
        {
            if (dataHourElements == 24)
            {
                throw new ArgumentException("Cannot add more data, day has 24 hours.");
            }

            dataByHour[dataHourElements++] = hourData;
        }

        public IEnumerator<WeatherHourData> GetEnumerator()
        {
            return new WeatherDataEnumerator(dataByHour);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new WeatherDataEnumerator(dataByHour);
        }

        public WeatherHourData this[int hour]
        {
            get
            {
                return dataByHour[hour];
            }
        }
    }
}
