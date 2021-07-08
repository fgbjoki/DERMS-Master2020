using Core.Common.AbstractModel;
using System.Runtime.Serialization;

namespace Core.Common.Transaction.Models
{
    [DataContract]
    public class IdentifiedObject
    {
        public IdentifiedObject()
        {

        }

        public IdentifiedObject(long globalId)
        {
            GlobalId = globalId;
            DMSType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(globalId);
        }

        [DataMember]
        public long GlobalId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public DMSType DMSType
        {
            get;
            set;
        }
    }
}
