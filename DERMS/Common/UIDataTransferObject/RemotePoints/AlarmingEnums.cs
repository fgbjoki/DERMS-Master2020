using System.Runtime.Serialization;

namespace Common.UIDataTransferObject.RemotePoints
{
    [DataContract]
    public enum AnalogAlarming
    {
        [EnumMember]
        NO_ALARM,
        [EnumMember]
        HIGH_ALARM,
        [EnumMember]
        LOW_ALARM
    }

    [DataContract]
    public enum DiscreteAlarming
    {
        [EnumMember]
        NO_ALARM,
        [EnumMember]
        ABNORMAL_ALARM
    }
}
