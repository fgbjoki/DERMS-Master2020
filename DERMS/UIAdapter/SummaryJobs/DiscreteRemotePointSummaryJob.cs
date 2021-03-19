using Common.ComponentStorage;
using Common.UIDataTransferObject.RemotePoints;
using UIAdapter.Model;

namespace UIAdapter.SummaryJobs
{
    public class DiscreteRemotePointSummaryJob : SummaryJob<DiscreteRemotePoint, DiscreteRemotePointSummaryDTO>
    {
        public DiscreteRemotePointSummaryJob(Storage<DiscreteRemotePoint> storage) : base(storage)
        {
        }
    }
}
