namespace UIAdapter.TransactionProcessing.Storages.Helpers
{
    public interface IAnalogEntityStorage
    {
        void UpdateAnalogValue(long measurementGid, float newValue);
    }
}
