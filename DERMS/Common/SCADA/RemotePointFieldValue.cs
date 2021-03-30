using System.Runtime.Serialization;

namespace Common.SCADA
{
    [DataContract]
    public class RemotePointFieldValue
    {
        public RemotePointFieldValue(long globalId, int value)
        {
            GlobalId = globalId;
            Value = value;
        }

        [DataMember]
        public long GlobalId { get; private set; }

        [DataMember]
        public int Value { get; private set; }
    }
}
