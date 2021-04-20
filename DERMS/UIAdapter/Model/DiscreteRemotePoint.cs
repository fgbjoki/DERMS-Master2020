using Common.UIDataTransferObject.RemotePoints;

namespace UIAdapter.Model
{
    public class DiscreteRemotePoint : RemotePoint<DiscreteRemotePointSummaryDTO>
    {
        private int value;

        public DiscreteRemotePoint(long globalId) : base(globalId)
        {
        }

        public int Value
        {
            get { return value; }
            set
            {
                this.value = value;
                if(this.value.Equals(NormalValue))
                {
                    Alarm = DiscreteAlarming.NO_ALARM;
                }
                else
                {
                    Alarm = DiscreteAlarming.ABNORMAL_ALARM;
                }
            }
        }

        public int NormalValue { get; set; }

        public int DOMManipulation { get; set; }

        public DiscreteAlarming Alarm { get; set; }

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
            dto.Alarm = Alarm;
        }
    }
}
