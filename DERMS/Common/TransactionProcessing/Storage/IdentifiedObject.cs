using Common.AbstractModel;

namespace Common.ComponentStorage
{
    public class IdentifiedObject
    {
        public IdentifiedObject(long globalId)
        {
            GlobalId = globalId;
            DMSType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(globalId);
        }

        public long GlobalId { get; private set; }

        public string Name { get; set; }

        public DMSType DMSType
        {
            get;
            private set;
        }

        public virtual void Update(IdentifiedObject entity)
        {

        }
    }
}
