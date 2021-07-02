using Core.Common.AbstractModel;
using System;
using System.Runtime.Serialization;

namespace Core.Common.PropertyWrappers.EnumWrappers
{
    public class DMSTypeWrapper : IEquatable<DMSTypeWrapper>, IComparable<DMSTypeWrapper>
    {
        public DMSTypeWrapper(DMSType dmsType)
        {
            Value = dmsType;
        }

        public DMSTypeWrapper()
        {

        }

        public DMSType Value { get; set; }


        public int CompareTo(DMSTypeWrapper other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool Equals(DMSTypeWrapper other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as DMSTypeWrapper);
        }
    }
}
