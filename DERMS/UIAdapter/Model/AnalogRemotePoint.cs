using Common.UIDataTransferObject.RemotePoints;

namespace UIAdapter.Model
{
    public class AnalogRemotePoint : RemotePoint
    {
        public AnalogRemotePoint(long globalId) : base(globalId)
        {
        }

        public float Value { get; set; }

        public AnalogRemotePointSummaryDTO CreateDTO()
        {
            AnalogRemotePointSummaryDTO dto = new AnalogRemotePointSummaryDTO();

            PopulateDTO(dto);
            dto.Value = Value;

            return dto;
        }
    }
}
