using System.Runtime.Serialization;

namespace Core.Common.Transaction.Models.FEP.FEPStorage
{
    [DataContract]
    public class RemotePoint : IdentifiedObject
    {
        public RemotePoint(long gid, ushort address, RemotePointType type) : base(gid)
        {
            Address = address;
            Type = type;
        }

        [DataMember]
        public ushort Address { get; private set; }

        [DataMember]
        public RemotePointType Type { get; private set; }
    }
}
