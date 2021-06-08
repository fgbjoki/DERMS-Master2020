using System.Runtime.Serialization;

namespace Common.UIDataTransferObject.DEROptimalCommanding
{
    [DataContract]
    public enum CommandRequestDTO
    {
        [EnumMember]
        NominalPower,
        [EnumMember]
        Reserve
    }
}
