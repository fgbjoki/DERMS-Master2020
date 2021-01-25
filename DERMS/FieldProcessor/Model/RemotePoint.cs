using Common.ComponentStorage;

namespace FieldProcessor.Model
{
    public class RemotePoint : IdentifiedObject
    {
        public RemotePoint(long gid, ushort address, RemotePointType type) : base(gid)
        {
            Address = address;
            Type = type;
        }

        public ushort Address { get; private set; }

        public RemotePointType Type { get; private set; }
    }
}
