using Common.UIDataTransferObject.RemotePoints;

namespace UIAdapter.Model
{
    public class DiscreteRemotePoint : RemotePoint<DiscreteRemotePointSummaryDTO>
    {
        public DiscreteRemotePoint(long globalId) : base(globalId)
        {
        }

        public int Value { get; set;}

        public int NormalValue { get; set; }

        public int DOMManipulation { get; set; }

        public override DiscreteRemotePointSummaryDTO CreateDTO()
        {
            DiscreteRemotePointSummaryDTO dto = new DiscreteRemotePointSummaryDTO();

            PopulateDTO(dto);
            
            return dto;
        }

        protected override void PopulateDTO(DiscreteRemotePointSummaryDTO dto)
        {
            base.PopulateDTO(dto);

            dto.Value = Value;
            dto.NormalValue = NormalValue;
            dto.DOMManipulation = DOMManipulation;
        }
    }
}
