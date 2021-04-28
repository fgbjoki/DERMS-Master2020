namespace Common.TransactionProcessing.Storage.Helpers
{
    public interface IAnalogEntityStorage
    {
        void UpdateAnalogValue(long measurementGid, float newValue);
    }
}
