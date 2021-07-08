using System.Runtime.Serialization;

namespace Core.Common.Transaction.Models.FEP.FEPStorage
{
    [DataContract]
    public class DiscreteRemotePoint : RemotePoint
    {
        public DiscreteRemotePoint()
        {

        }
        public DiscreteRemotePoint(long gid, ushort address, RemotePointType type) : base(gid, address, type)
        {
        }
    }
}
