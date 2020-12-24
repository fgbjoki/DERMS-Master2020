using FieldSimulator.Model;

namespace FieldSimulator.ViewModel
{
    class InputRegistersViewModel : RemotePointsViewModel<InputRegisterWrapper>
    {
        public InputRegistersViewModel(InputRegisterWrapper[] remotePoints) : base("Input Registers", remotePoints)
        {
        }
    }
}
