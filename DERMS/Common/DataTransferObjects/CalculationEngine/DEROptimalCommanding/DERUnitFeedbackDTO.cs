using System.Runtime.Serialization;

namespace Common.DataTransferObjects.CalculationEngine.DEROptimalCommanding
{
    [DataContract]
    public class DERUnitFeedbackDTO
    {
        [DataMember]
        public long DERGlobalId { get; set; }

        [DataMember]
        public float ActivePower { get; set; }

        [DataMember]
        public CommandFeedback CommandFeedback { get; set; }
    }
}
