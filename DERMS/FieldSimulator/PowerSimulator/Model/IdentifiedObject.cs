using Common.AbstractModel;

namespace FieldSimulator.PowerSimulator.Model
{
    public abstract class IdentifiedObject
    {
        public IdentifiedObject(long globalId)
        {
            GlobalId = globalId;

            DMSType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(globalId);
        }

        public long GlobalId { get; set; }

        public DMSType DMSType { get; set; }

        public string Name { get; set; }

        public virtual void Update(DERMS.IdentifiedObject cimObject)
        {
            Name = cimObject.Name;
        }
    } 
}
