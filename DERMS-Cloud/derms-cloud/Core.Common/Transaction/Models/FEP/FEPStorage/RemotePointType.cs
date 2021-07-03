using System.Runtime.Serialization;

namespace Core.Common.Transaction.Models.FEP.FEPStorage
{
    [DataContract]
    public enum RemotePointType : ushort
    {
        [EnumMember]
        Coil = 0,
        [EnumMember]
        DiscreteInput = 1,
        [EnumMember]
        HoldingRegister = 2,
        [EnumMember]
        InputRegister = 3,
    }
}
