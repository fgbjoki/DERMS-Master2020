using System;
using System.Runtime.Serialization;

namespace FEPStorage.Model
{
    [DataContract]
    public class RemotePointTypeWrapper : IEquatable<RemotePointTypeWrapper>, IComparable<RemotePointTypeWrapper>
    {
        public RemotePointTypeWrapper(RemotePointType dmsType)
        {
            Value = dmsType;
        }

        public RemotePointTypeWrapper()
        {

        }

        [DataMember]
        public RemotePointType Value { get; set; }


        public int CompareTo(RemotePointTypeWrapper other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool Equals(RemotePointTypeWrapper other)
        {
            return Value.Equals(other.Value);
        }
    }
}
