using Common.ComponentStorage;

namespace NetworkDynamicsService.Model.RemotePoints
{
    public class RemotePoint : IdentifiedObject
    {
        public RemotePoint(long globalId, int address) : base(globalId)
        {
            Address = address;
        }

        public int Address { get; set; }
    }
}
