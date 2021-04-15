namespace FieldSimulator.Model
{
    public class CoilWrapper : DiscretePointWrapper
    {
        public CoilWrapper(int index) : base(RemotePointType.Coil, index)
        {
        }
    }
}
