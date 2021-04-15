using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FieldSimulator.Model
{
    public enum RemotePointType : short
    {
        Coil = 1,
        DiscreteInput = 2,
        HoldingRegister = 3,
        InputRegister = 4
    }

    public abstract class BasePoint : INotifyPropertyChanged
    {
        private short pointValue;

        protected string name;

        protected short address;

        protected RemotePointType pointType;

        public BasePoint(RemotePointType pointType, int index)
        {
            Address = (short)index;
            this.pointType = pointType;
        }

        public short Address
        {
            get { return address; }
            set { SetProperty(ref address, value); }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    SetProperty(ref name, value);
                }
            }
        }

        public long GlobalId { get; set; }

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

        public override int GetHashCode()
        {
            return ((ushort)pointType) | (Address << sizeof(ushort) * 8);
        }

        public override bool Equals(object obj)
        {
            BasePoint basePoint = obj as BasePoint;

            return basePoint != null && pointType == basePoint.pointType && address == basePoint.address;
        }
    }
}
