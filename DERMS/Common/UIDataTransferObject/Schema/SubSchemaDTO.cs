using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Common.UIDataTransferObject.Schema
{
    [DataContract]
    public class SubSchemaDTO : IdentifiedObjectDTO
    {
        [DataMember]
        public long EnergySourceGid { get; set; }

        [DataMember]
        public long InterConnectedBreaker { get; set; }

        [DataMember]
        public Dictionary<long, List<long>> ParentChildRelation { get; set; }

        [DataMember]
        public SubSchemaConductingEquipmentEnergized ConductingEquipmentStates { get; set; }
    }
}
