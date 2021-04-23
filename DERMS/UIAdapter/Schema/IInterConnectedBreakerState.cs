namespace UIAdapter.Schema
{
    public interface IInterConnectedBreakerState
    {
        bool DoesInterConnectedBreakerConduct(long energySourceGid);
    }
}