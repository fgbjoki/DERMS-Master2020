using System.Runtime.Serialization;

namespace FEPStorage.Model
{
    [DataContract]
    public class AnalogRemotePoint : RemotePoint
    {
        public AnalogRemotePoint(long gid, ushort address, RemotePointType type) : base(gid, address, type)
        {
        }
    }
}
