namespace FieldSimulator.PowerSimulator.Storage.Weather
{
    interface IWeatherDataContainer : IWeatherDataStorage
    {
        void FetchNextHourData();
    }
}