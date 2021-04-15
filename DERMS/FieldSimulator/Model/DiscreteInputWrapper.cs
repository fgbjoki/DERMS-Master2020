namespace FieldSimulator.Model
{
    public class DiscreteInputWrapper : DiscretePointWrapper
    {
        public DiscreteInputWrapper(int index) : base(RemotePointType.DiscreteInput, index)
        {
        }
    }
}
