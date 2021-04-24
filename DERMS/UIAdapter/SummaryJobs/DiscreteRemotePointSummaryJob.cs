using Common.ComponentStorage;
using Common.UIDataTransferObject.RemotePoints;
using UIAdapter.Model;

namespace UIAdapter.SummaryJobs
{
    public class DiscreteRemotePointSummaryJob : SummaryJob<DiscreteRemotePoint, DiscreteRemotePointSummaryDTO>
    {
        public DiscreteRemotePointSummaryJob(IStorage<DiscreteRemotePoint> storage) : base(storage)
        {
        }
    }
}
