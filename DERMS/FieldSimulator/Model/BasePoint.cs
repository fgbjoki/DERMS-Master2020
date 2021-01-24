using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FieldSimulator.Model
{
    public enum PointType
    {
        Coil,
        DiscreteInput,
        HoldingRegister,
        InputRegister
    }

    public delegate void PointValueChanged(PointType pointType, int index, short value);

    public abstract class BasePoint : INotifyPropertyChanged
    {
        private short pointValue;

        private int index;

        private PointType pointType;

        public BasePoint(PointType pointType, int index)
        {
            Index = index;
            this.pointType = pointType;
        }

        public short Value
        {
            get { return pointValue; }
            set
            {
                SetProperty(ref pointValue, value);
                PointValueChanged.Invoke(pointType, index, pointValue);
            }
        }

        public int Index
        {
            get { return index - 1; }
            set { SetProperty(ref index, value); }
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

        public event PointValueChanged PointValueChanged = delegate { };

        #endregion
    }
}
