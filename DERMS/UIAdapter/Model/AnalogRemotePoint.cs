using Common.UIDataTransferObject.RemotePoints;

namespace UIAdapter.Model
{
    public class AnalogRemotePoint : RemotePoint<AnalogRemotePointSummaryDTO>
    {
        private float value;

        public AnalogRemotePoint(long globalId) : base(globalId)
        {
        }

        public float Value
        {
            get { return value; }
            set
            {
                this.value = value;
                if (this.value >= MaxValue)
                {
                    Alarm = AnalogAlarming.HIGH_ALARM;
                }
                else if (this.value <= MinValue)
                {
                    Alarm = AnalogAlarming.LOW_ALARM;
                }
                else
                {
                    Alarm = AnalogAlarming.NO_ALARM;
                }
            }
        }

        public float MaxValue { get; set; }

        public float MinValue { get; set; }

        public AnalogAlarming Alarm { get; set; }

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
            dto.MaxValue = MaxValue;
            dto.MinValue = MinValue;
            dto.Alarm = Alarm;
        }
    }
}
