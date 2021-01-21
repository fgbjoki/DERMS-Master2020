using System.Runtime.Serialization;

namespace Common.SCADA
{
    [DataContract]
    public class RemotePointFieldValue
    {
        public RemotePointFieldValue(long globalId, ushort value)
        {
            GlobalId = globalId;
            Value = value;
        }

        [DataMember]
        public long GlobalId { get; private set; }

        [DataMember]
        public ushort Value { get; private set; }
    }
}
