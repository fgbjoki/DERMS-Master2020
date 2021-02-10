using Common.UIDataTransferObject.RemotePoints;
using UIAdapter.SummaryJobs;

namespace UIAdapter.Model
{
    public abstract class RemotePoint<T> : SummaryItem<T>
        where T : RemotePointSummaryDTO
    {
        public RemotePoint(long globalId) : base(globalId)
        {
        }

        public string Name { get; set; }

        public int Address { get; set; }

        protected virtual void PopulateDTO(T dto)
        {
            dto.Address = Address;
            dto.GlobalId = GlobalId;
            dto.Name = Name;
        }
    }
}
