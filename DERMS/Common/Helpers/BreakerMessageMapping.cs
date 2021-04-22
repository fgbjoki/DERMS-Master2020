namespace Common.Helpers.Breakers
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

        public int MapBreakerStateToRawData(BreakerState breakerState)
        {
            switch (breakerState)
            {
                case BreakerState.CLOSED:
                    return 0;
                case BreakerState.OPEN:
                    return 1;
                default:
                    return -1;
            }
        }
    }
}
