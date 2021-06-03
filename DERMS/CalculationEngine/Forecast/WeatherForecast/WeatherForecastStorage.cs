using CalculationEngine.Forecast.WeatherForecast.DataConverter;
using Common.Logger;
using Common.WeatherAPI;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;

namespace CalculationEngine.Forecast.WeatherForecast
{
    public class WeatherForecastStorage : IWeatherForecastStorage
    {
        private static readonly int lockerTimeout = 2000;

        private readonly int minutesADay = 24 * 60;
        private IWeatherClient weatherClient;
        private System.Timers.Timer fetchDataTimer;
        private System.Timers.Timer moveCurrentWeatherTimer;

        private List<WeatherDataInfo> weatherData;
        private int currentWeatherData = 0;

        private ReaderWriterLock locker = new ReaderWriterLock();

        private IWeatherDataConverter dataWeatherConterver;

        private bool initialization = true;

        public WeatherForecastStorage(IWeatherClient weatherClient)
        {
            weatherData = new List<WeatherDataInfo>(72 * 60);

            this.weatherClient = weatherClient;

            dataWeatherConterver = new WeatherDataConverter();

            InitializeTimers();
        }

        private void InitializeTimers()
        {
            fetchDataTimer = new System.Timers.Timer();
            fetchDataTimer.AutoReset = true;
            fetchDataTimer.Elapsed += FetchWeatherData;

            FetchWeatherData(this, null);

            moveCurrentWeatherTimer = new System.Timers.Timer();
            moveCurrentWeatherTimer.AutoReset = true;
            moveCurrentWeatherTimer.Elapsed += MoveCurrentWeather;
            moveCurrentWeatherTimer.Enabled = true;
        }

        private void MoveCurrentWeather(object sender, ElapsedEventArgs e)
        {
            moveCurrentWeatherTimer.Enabled = false;

            moveCurrentWeatherTimer.Interval = RecalculateNextMinuteRemainingMilliseconds();

            locker.AcquireWriterLock(lockerTimeout);

            if (currentWeatherData == minutesADay - 1)
            {
                currentWeatherData = 0;
                weatherData.RemoveRange(0, minutesADay - 1);
            }
            else
            {
                ++currentWeatherData;
            }

            locker.ReleaseWriterLock();

            moveCurrentWeatherTimer.Enabled = true;
        }

        private void FetchWeatherData(object sender, ElapsedEventArgs e)
        {
            DateTime now = DateTime.Now;
            fetchDataTimer.Enabled = false;

            fetchDataTimer.Interval = RecalculateFetchRemainingMilliseconds();

            locker.AcquireWriterLock(lockerTimeout);

            Logger.Instance.Log($"[{GetType().Name}] Fetching weather data from weather api.");
            List<WeatherDayData> weatherDayData = weatherClient.GetWeatherData(3);
            Logger.Instance.Log($"[{GetType().Name}] Fetched {weatherDayData.Count} days of weather data.");

            Logger.Instance.Log($"[{GetType().Name}] Interpolating weather data on minute intervals.");
            weatherData.AddRange(dataWeatherConterver.CovertData(weatherDayData));
            Logger.Instance.Log($"[{GetType().Name}] Interpolation sucessful.");

            InitializeCurrentWeahterPointer(now);

            locker.ReleaseWriterLock();

            fetchDataTimer.Enabled = true;
        }

        private double RecalculateFetchRemainingMilliseconds()
        {
            DateTime now = DateTime.Now;
            TimeSpan nextFetchDataDay = new TimeSpan(2, 0, 0, 0, 10) - new TimeSpan(now.Hour, now.Minute, now.Second);

            return nextFetchDataDay.TotalMilliseconds;
        }

        private double RecalculateNextMinuteRemainingMilliseconds()
        {
            DateTime now = DateTime.Now;
            TimeSpan nextMinute = TimeSpan.FromMinutes(1) - new TimeSpan(0, 0, 0, now.Second, 10); 

            return nextMinute.TotalMilliseconds;
        }

        private void InitializeCurrentWeahterPointer(DateTime now)
        {
            if (!initialization)
            {
                return;
            }

            initialization = false;

            currentWeatherData = 60 * now.Hour + now.Minute;
        }

        public List<WeatherDataInfo> GetMinutesWeatherInfo(int minutes)
        {
            List<WeatherDataInfo> info;

            locker.AcquireReaderLock(lockerTimeout);

            if (minutes + currentWeatherData >= weatherData.Count)
            {
                locker.ReleaseReaderLock();
                return new List<WeatherDataInfo>();
            }

            info = weatherData.GetRange(currentWeatherData, minutes);

            locker.ReleaseReaderLock();

            return info;
        }

        public List<WeatherDataInfo> GetHourlyWeatherInfo(int hours)
        {
            List<WeatherDataInfo> info = new List<WeatherDataInfo>(hours + 1);

            locker.AcquireReaderLock(lockerTimeout);

            if (hours * 60 + currentWeatherData >= weatherData.Count)
            {
                locker.ReleaseReaderLock();
                return new List<WeatherDataInfo>();
            }

            int reverseMinutes = weatherData[currentWeatherData].CurrentTime.Minute;

            int firstHourIndex = currentWeatherData - reverseMinutes;

            for (int i = firstHourIndex, j = 0; j < hours + 1; i += 60, ++j)
            {
                info.Add(weatherData[i]);
            }

            locker.ReleaseReaderLock();

            return info;
        }
    }
}
