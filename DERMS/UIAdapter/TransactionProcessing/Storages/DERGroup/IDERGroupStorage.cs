namespace UIAdapter.TransactionProcessing.Storages.DERGroup
{
    public interface IDERGroupStorage
    {
        void UpdateDERState(long derGid, float activePower);
    }
}