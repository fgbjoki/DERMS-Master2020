using FieldSimulator.Model;
using System.Collections.ObjectModel;

namespace FieldSimulator.ViewModel
{
    public abstract class RemotePointsViewModel<T> : BaseViewModel 
        where T : BasePoint
    {
        public RemotePointsViewModel(string viewModelLabel, T[] remotePoints) : base(viewModelLabel)
        {
            RemotePoints = new ObservableCollection<T>();

            foreach (T remotePoint in remotePoints)
            {
                RemotePoints.Add(remotePoint);
            }
        }

        public ObservableCollection<T> RemotePoints { get; set; }
    }
}
