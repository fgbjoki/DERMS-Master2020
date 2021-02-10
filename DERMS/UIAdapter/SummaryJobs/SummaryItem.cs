using Common.ComponentStorage;

namespace UIAdapter.SummaryJobs
{
    public abstract class SummaryItem<T> : IdentifiedObject
    {
        public SummaryItem(long globalId) : base(globalId)
        {
        }

        public abstract T CreateDTO();
    }
}
