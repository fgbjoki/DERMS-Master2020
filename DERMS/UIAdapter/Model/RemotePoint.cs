using Common.ComponentStorage;
using Common.UIDataTransferObject.RemotePoints;

namespace UIAdapter.Model
{
    public abstract class RemotePoint : IdentifiedObject
    {
        public RemotePoint(long globalId) : base(globalId)
        {
        }

        public string Name { get; set; }

        public int Address { get; set; }

        public protected void PopulateDTO(RemotePointSummaryDTO dto)
        {
            dto.Address = Address;
            dto.GlobalId = GlobalId;
            dto.Name = Name;
        }
    }
}
