using Core.Common.Transaction.Models.FEP.FEPStorage;
using System.Runtime.Serialization;

namespace FEPStorage.Model
{
    [DataContract]
    public class DiscreteRemotePoint : RemotePoint
    {
        public DiscreteRemotePoint(long gid, ushort address, RemotePointType type) : base(gid, address, type)
        {
        }
    }
}
