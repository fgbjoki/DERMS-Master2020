using FieldSimulator.Model;

namespace FieldSimulator.ViewModel
{
    public class CoilsViewModel : RemotePointsViewModel<CoilWrapper>
    {
        public CoilsViewModel(CoilWrapper[] coils) : base("Coils", coils)
        {
        }
    }
}
