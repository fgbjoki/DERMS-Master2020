using Common.UIDataTransferObject.RemotePoints;

namespace UIAdapter.Model
{
    public class AnalogRemotePoint : RemotePoint<AnalogRemotePointSummaryDTO>
    {
        public AnalogRemotePoint(long globalId) : base(globalId)
        {
        }

        public float Value { get; set; }

        public override AnalogRemotePointSummaryDTO CreateDTO()
        {
            AnalogRemotePointSummaryDTO dto = new AnalogRemotePointSummaryDTO();

            PopulateDTO(dto);
           
            return dto;
        }

        protected override void PopulateDTO(AnalogRemotePointSummaryDTO dto)
        {
            base.PopulateDTO(dto);
            dto.Value = Value;
        }
    }
}
