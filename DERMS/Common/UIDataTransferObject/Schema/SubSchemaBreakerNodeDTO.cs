using System.Runtime.Serialization;

namespace Common.UIDataTransferObject.Schema
{
    [DataContract]
    public class SubSchemaBreakerNodeDTO : SubSchemaNodeDTO
    {
        [DataMember]
        public bool Closed { get; set; }
    }
}
