using System.Runtime.Serialization;

namespace Core.Common.Transaction.Models.FEP.FEPStorage
{
    [DataContract]
    public class AnalogRemotePoint : RemotePoint
    {
        public AnalogRemotePoint()
        {

        }
        public AnalogRemotePoint(long gid, ushort address, RemotePointType type) : base(gid, address, type)
        {
        }
    }
}
