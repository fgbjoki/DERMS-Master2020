using Common.UIDataTransferObject.NetworkModel;
using UIAdapter.SummaryJobs;

namespace UIAdapter.Model.NetworkModel
{
    public class NetworkModelItem : SummaryItem<NetworkModelEntityDTO>
    {
        public NetworkModelItem(long globalId) : base(globalId)
        {
        }

        public override NetworkModelEntityDTO CreateDTO()
        {
            return new NetworkModelEntityDTO()
            {
                GlobalId = GlobalId,
                Name = Name
            };
        }
    }
}
