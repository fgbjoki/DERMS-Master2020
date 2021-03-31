using System.Runtime.Serialization;

namespace Common.UIDataTransferObject.Schema
{
    [DataContract]
    public class EnergySourceDTO : IdentifiedObjectDTO
    {
        [DataMember]
        public string SubstationName { get; set; }

        [DataMember]
        public long SubstationGid { get; set; }
    }
}
