using System.Runtime.Serialization;

namespace Common.DataTransferObjects.CalculationEngine
{
    [DataContract]
    public class CommandFeedback
    {
        [DataMember]
        public bool Successful { get; set; }

        [DataMember]
        public string Message { get; set; }
    }
}
