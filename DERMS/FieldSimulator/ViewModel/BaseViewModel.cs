using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FieldSimulator.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private string label;

        public BaseViewModel(string viewModelLabel)
        {
            label = viewModelLabel;
        }

        public string ViewModelLabel
        {
            get { return label; }
            set { SetProperty(ref label, value); }
        }

        #region PropertyChanged

        protected virtual void SetProperty<T>(ref T member, T val,
           [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(member, val)) return;

            member = val;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion
    }
}
