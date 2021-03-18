namespace CalculationEngine.TransactionProcessing.Storage
{
    public interface IStorageDependentUnit<StorageType>
    {
        bool Prepare(StorageType storage);
        bool Commit();
        bool Rollback();
    }
}
