using System.Collections.Generic;

namespace Common.WeatherAPI
{
    class WeatherDataEnumerator : IEnumerator<WeatherHourData>
    {
        private WeatherHourData[] dataByHour;
        private int currentElement = -1;

        public WeatherDataEnumerator(WeatherHourData[] dataByHour)
        {
            this.dataByHour = dataByHour;
        }

        public object Current
        {
            get
            {
                return dataByHour[currentElement];
            }
        }

        WeatherHourData IEnumerator<WeatherHourData>.Current
        {
            get
            {
                return dataByHour[currentElement];
            }
        }

        public bool MoveNext()
        {
            if (currentElement == dataByHour.Length - 1)
            {
                return false;
            }

            currentElement++;
            return true;
        }

        public void Reset()
        {
            currentElement = -1;
        }

        public void Dispose()
        {

        }
    }
}
