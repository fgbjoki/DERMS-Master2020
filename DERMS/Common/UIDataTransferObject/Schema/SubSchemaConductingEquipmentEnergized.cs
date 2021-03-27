using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Common.UIDataTransferObject.Schema
{
    [DataContract]
    public class SubSchemaConductingEquipmentEnergized
    {
        [DataMember]
        public Dictionary<long, SubSchemaNodeDTO> Nodes { get; set; }
    }
}
