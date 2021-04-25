namespace Common.ComponentStorage
{
    public abstract class IdentifiedObject
    {
        public IdentifiedObject(long globalId)
        {
            GlobalId = globalId;
        }

        public long GlobalId { get; private set; }

        public string Name { get; set; }

        public virtual void Update(IdentifiedObject entity)
        {

        }
    }
}
