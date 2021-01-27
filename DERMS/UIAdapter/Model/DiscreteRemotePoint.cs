using Common.UIDataTransferObject.RemotePoints;

namespace UIAdapter.Model
{
    public class DiscreteRemotePoint : RemotePoint
    {
        public DiscreteRemotePoint(long globalId) : base(globalId)
        {
        }

        public int Value { get; set;}

        public int NormalValue { get; set; }

        public DiscreteRemotePointSummaryDTO CreateDTO()
        {
            DiscreteRemotePointSummaryDTO dto = new DiscreteRemotePointSummaryDTO();

            PopulateDTO(dto);
            dto.Value = Value;
            dto.NormalValue = NormalValue;

            return dto;
        }
    }
}
