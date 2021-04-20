namespace FieldSimulator.PowerSimulator.Storage.Weather
{
    interface IWeatherDataStorage
    {
        float CloudCover { get; }
        bool IsSunny { get; }
        float Temperature { get; }
        float WindKPH { get; }
        StorageLock StorageLock { get; }
    }
}
