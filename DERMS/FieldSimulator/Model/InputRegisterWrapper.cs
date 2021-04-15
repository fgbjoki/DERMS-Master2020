namespace FieldSimulator.Model
{
    public class InputRegisterWrapper : AnalogPointWrapper
    {
        public InputRegisterWrapper(int index) : base(RemotePointType.InputRegister, index)
        {
        }
    }
}
