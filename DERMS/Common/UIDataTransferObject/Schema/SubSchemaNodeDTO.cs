using System.Runtime.Serialization;

namespace Common.UIDataTransferObject.Schema
{
    [DataContract]
    public class SubSchemaNodeDTO : IdentifiedObjectDTO
    {
        [DataMember]
        public bool IsEnergized { get; set; }

        [DataMember]
        public bool DoesConduct { get; set; }
    }
}
