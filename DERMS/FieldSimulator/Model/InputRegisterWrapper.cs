namespace FieldSimulator.Model
{
    class InputRegisterWrapper : AnalogPointWrapper
    {
        public InputRegisterWrapper(int index) : base(PointType.InputRegister, index)
        {
        }
    }
}
