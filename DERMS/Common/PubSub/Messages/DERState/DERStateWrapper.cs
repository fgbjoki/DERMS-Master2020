namespace Common.PubSub.Messages.DERState
{
    public class DERStateWrapper
    {
        public DERStateWrapper()
        {

        }

        public DERStateWrapper(long globalId, float activePower, long connectedSourceGid)
        {
            GlobalId = globalId;
            ActivePower = activePower;
            ConnectedSourceGid = connectedSourceGid;
        }

        public long GlobalId { get; set; }
        public float ActivePower { get; set; }
        public long ConnectedSourceGid { get; set; }
    }
}
