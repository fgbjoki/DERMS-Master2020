namespace Common.AbstractModel
{
    public enum MeasurementType : short
    {
        ActiveEnergy = 0,
        ActivePower = 1,
        Discrete = 2,
        Humidity = 3,
        Money = 4,
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

    public enum SignalDirection: short
    {
        Read = 1,
        Write = 2,
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
