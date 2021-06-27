using System.Runtime.Serialization;

namespace Core.Common.AbstractModel
{
    public enum MeasurementType : short
    {
        [EnumMember]
        ActiveEnergy = 0,
        [EnumMember]
        ActivePower = 1,
        [EnumMember]
        Discrete = 2,
        Humidity = 3,
        Money = 4,
        [EnumMember]
        Percent = 5,
        SkyCover = 7,
        Status = 8,
        SunshineMinutes = 9,
        Temperature = 10,
        Time = 11,
        Unitless = 12,
        WindSpeed = 13,
        DeltaPower = 14
    }

    [DataContract]
    public enum SignalDirection: short
    {
        [EnumMember]
        Read = 1,
        [EnumMember]
        Write = 2,
        [EnumMember]
        ReadWrite = 3,
    }

    public enum EnergyStorageState : short
    {
        Idle = 0,
        Charging = 1,
        Discharging = 2
    }

    public enum DiscreteType : short
    {
        Coil = 0,
        DiscreteInput = 1
    }

    public enum AnalogType : short
    {
        HoldingRegister = 0,
        InputRegister = 1
    }
}
