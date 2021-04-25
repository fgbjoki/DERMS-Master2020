using Common.UIDataTransferObject.DERGroup;
using UIAdapter.Model.DERGroup;
using Common.ComponentStorage;

namespace UIAdapter.SummaryJobs
{
    public class DERGroupSummaryJob : SummaryJob<DERGroup, DERGroupSummaryDTO>
    {
        public DERGroupSummaryJob(IStorage<DERGroup> storage) : base(storage)
        {
        }
    }
}
