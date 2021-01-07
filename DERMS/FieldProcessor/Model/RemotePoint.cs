namespace FieldProcessor.Model
{
    public class RemotePoint
    {
        public RemotePoint(long gid, ushort address, RemotePointType type)
        {
            Gid = gid;
            Address = address;
            Type = type;
        }

        public long Gid { get; private set; }

        public ushort Address { get; private set; }

        public RemotePointType Type { get; private set; }
    }
}
