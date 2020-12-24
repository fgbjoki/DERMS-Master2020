using FieldSimulator.Model;

namespace FieldSimulator.ViewModel
{
    public class HoldingRegistersViewModel : RemotePointsViewModel<HoldingRegisterWrapper>
    {
        public HoldingRegistersViewModel(HoldingRegisterWrapper[] holdingRegisters) : base("Holding Registers", holdingRegisters)
        {
        }
    }
}
