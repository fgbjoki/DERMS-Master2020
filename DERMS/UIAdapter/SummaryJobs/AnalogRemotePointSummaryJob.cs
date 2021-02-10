using Common.UIDataTransferObject.RemotePoints;
using UIAdapter.Model;
using Common.ComponentStorage;

namespace UIAdapter.SummaryJobs
{
    public class AnalogRemotePointSummaryJob : SummaryJob<AnalogRemotePoint, AnalogRemotePointSummaryDTO>
    {
        public AnalogRemotePointSummaryJob(Storage<AnalogRemotePoint> storage) : base(storage)
        {
        }
    }
}
