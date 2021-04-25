using System.Runtime.Serialization;

namespace Common.UIDataTransferObject.DERGroup
{
    [DataContract]
    public enum GeneratorType
    {
        [EnumMember]
        WIND,
        [EnumMember]
        SOLAR
    }

}
