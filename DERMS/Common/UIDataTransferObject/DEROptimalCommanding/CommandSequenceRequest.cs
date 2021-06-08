using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Common.UIDataTransferObject.DEROptimalCommanding
{
    [DataContract]
    public class CommandRequest
    {
        [DataMember]
        public long GlobalId { get; set; }

        [DataMember]
        public float ActivePower { get; set; }
    }

    [DataContract]
    public class CommandSequenceRequest
    {
        public CommandSequenceRequest()
        {
            CommandRequestSequence = new List<CommandRequest>();
        }

        [DataMember]
        public List<CommandRequest> CommandRequestSequence { get; set; }
    }
}
