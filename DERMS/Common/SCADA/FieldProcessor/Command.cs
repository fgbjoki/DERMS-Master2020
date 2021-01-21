using System.Runtime.Serialization;

namespace Common.SCADA.FieldProcessor
{
    [DataContract]
    public class Command
    {
        public Command(long globalId)
        {
            GlobalId = globalId;
        }

        [DataMember]
        public long GlobalId { get; private set; }
    }
}
