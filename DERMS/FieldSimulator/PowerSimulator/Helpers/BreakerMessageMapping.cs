namespace FieldSimulator.PowerSimulator.Helpers
{
    public enum BreakerState
    {
        CLOSED,
        OPEN,
        UNKNOWN
    }

    public class BreakerMessageMapping
    {
        public BreakerState MapRawDataToBreakerState(int rawValue)
        {
            switch (rawValue)
            {
                case 0:
                    return BreakerState.CLOSED;
                case 1:
                    return BreakerState.OPEN;
                default:
                    return BreakerState.UNKNOWN;
            }
        }
    }
}
