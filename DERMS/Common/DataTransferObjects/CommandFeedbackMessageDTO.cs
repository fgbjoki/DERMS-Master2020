using System.Runtime.Serialization;

namespace Common.DataTransferObjects
{
    [DataContract]
    public class CommandFeedbackMessageDTO
    {
        [DataMember]
        public bool CommandExecuted { get; set; }
        [DataMember]
        public string Message { get; set; }
    }
}
