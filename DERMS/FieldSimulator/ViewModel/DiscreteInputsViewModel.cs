using FieldSimulator.Model;

namespace FieldSimulator.ViewModel
{
    class DiscreteInputsViewModel : RemotePointsViewModel<DiscreteInputWrapper>
    {
        public DiscreteInputsViewModel(DiscreteInputWrapper[] remotePoints) : base("Discrete Input", remotePoints)
        {
        }
    }
}
