using System;

namespace Common.AbstractModel
{
    public enum DMSType : short
    {
        MASK_TYPE = unchecked((short)0xFFFF),
    }

    [Flags]
    public enum ModelCode : long
    {
        IDOBJ = 0x1000000000000000,
        IDOBJ_GID = 0x1000000000000104,  
    }

    [Flags]
    public enum ModelCodeMask : long
    {
        MASK_TYPE = 0x00000000ffff0000,
        MASK_ATTRIBUTE_INDEX = 0x000000000000ff00,
        MASK_ATTRIBUTE_TYPE = 0x00000000000000ff,

        MASK_INHERITANCE_ONLY = unchecked((long)0xffffffff00000000),
        MASK_FIRSTNBL = unchecked((long)0xf000000000000000),
        MASK_DELFROMNBL8 = unchecked((long)0xfffffff000000000),
    }
}
