namespace FieldSimulator.Model
{
    public class HoldingRegisterWrapper : AnalogPointWrapper
    {
        public HoldingRegisterWrapper(int index) : base(RemotePointType.HoldingRegister, index)
        {
        }
    }
}
