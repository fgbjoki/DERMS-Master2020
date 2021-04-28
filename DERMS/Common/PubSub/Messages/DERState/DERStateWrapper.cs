namespace Common.PubSub.Messages.DERState
{
    public class DERStateWrapper
    {
        public DERStateWrapper()
        {

        }

        public DERStateWrapper(long globalId, float activePower)
        {
            GlobalId = globalId;
            ActivePower = activePower;
        }

        public long GlobalId { get; set; }
        public float ActivePower { get; set; }
    }
}
