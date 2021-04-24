using Common.WeatherAPI;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;

namespace FieldSimulator.PowerSimulator.Storage.Weather
{
    public class WeatherStorage : IWeatherDataStorage
    {
        private ReaderWriterLockSlim locker;
        private System.Timers.Timer timer;
        private IWeatherClient weatherClient;
        private IWeatherDataContainer currentWeatherData;

        private List<WeatherDayData> daysData;

        public float CloudCover
        {
            get
            {
                return currentWeatherData.CloudCover;
            }
        }

        public bool IsSunny
        {
            get
            {
                return currentWeatherData.IsSunny;
            }
        }

        public float Temperature
        {
            get
            {
                return currentWeatherData.Temperature;
            }
        }

        public float WindKPH
        {
            get
            {
                return currentWeatherData.WindKPH;
            }
        }

        public StorageLock StorageLock
        {
            get
            {
                return new StorageLock(locker);
            }
        }

        public WeatherStorage(IWeatherClient weatherClient)
        {
            this.weatherClient = weatherClient;
            locker = new ReaderWriterLockSlim();

            timer = new System.Timers.Timer();
            timer.Interval = CalculateRemainingTimerMilliseconds();
            timer.Elapsed += LoadNewWeatherData;
            timer.Enabled = true;
            GetWeatherData();
        }

        private void GetWeatherData()
        {
            locker.EnterWriteLock();

            daysData = weatherClient.GetWeatherData(2);
            currentWeatherData = new WeatherDataContainer(daysData, DateTime.Now.Hour);

            locker.ExitWriteLock();
        }

        private void LoadNewWeatherData(object sender, ElapsedEventArgs e)
        {
            timer.Enabled = false;
            timer.Interval = CalculateRemainingTimerMilliseconds();
            timer.Enabled = true;

            locker.EnterWriteLock();

            currentWeatherData.FetchNextHourData();

            locker.ExitWriteLock();
        }

        private double CalculateRemainingTimerMilliseconds()
        {
            DateTime currentTime = DateTime.Now;
            DateTime nextHourTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, 0, 0, 0) + new TimeSpan(0, 1, 0, 0, 100);

            TimeSpan remainingTime = nextHourTime - currentTime;

            return remainingTime.TotalMilliseconds;
        }
    }
}
