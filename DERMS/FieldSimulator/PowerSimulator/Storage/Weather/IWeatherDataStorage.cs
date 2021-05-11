namespace FieldSimulator.PowerSimulator.Storage.Weather
{
    interface IWeatherDataStorage
    {
        float CloudCover { get; }
        bool IsSunny { get; }
        float Temperature { get; }
        float WindMPS { get; }
        StorageLock StorageLock { get; }
    }
}
