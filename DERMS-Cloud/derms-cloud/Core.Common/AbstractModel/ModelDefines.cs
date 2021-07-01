﻿using System;
using System.Runtime.Serialization;

namespace Core.Common.AbstractModel
{
    [DataContract]
    public class DMSTypeWrapper : IEquatable<DMSTypeWrapper>, IComparable<DMSTypeWrapper>, IEquatable<DMSType>
    {
        [DataMember]
        public DMSType DMSType { get; set; }

        public DMSTypeWrapper(DMSType dmsType)
        {
            DMSType = dmsType;
        }

        public DMSTypeWrapper()
        {

        }

        public int CompareTo(DMSTypeWrapper other)
        {
            return DMSType.CompareTo(other.DMSType);
        }

        public bool Equals(DMSTypeWrapper other)
        {
            return DMSType == other.DMSType;
        }

        public bool Equals(DMSType other)
        {
            return DMSType == other;
        }
    }

    public enum DMSType : short
    {
        MASK_TYPE = unchecked((short)0xFFFF),
        CONNECTIVITYNODE            = 0x0001,
        TERMINAL                    = 0x0002,
        SUBSTATION                  = 0x0003,
        GEOGRAPHICALREGION          = 0x0004,
        SUBGEOGRAPHICALREGION       = 0x0005,
        MEASUREMENTDISCRETE         = 0x0006,
        MEASUREMENTANALOG           = 0x0007,
        ENERGYSTORAGE               = 0x0008,
        SOLARGENERATOR              = 0x0009,
        WINDGENERATOR               = 0x000A,
        ENERGYCONSUMER              = 0x000B,
        ENERGYSOURCE                = 0x000C,
        ACLINESEG                   = 0x000D,
        BREAKER                     = 0x000E,
    }

    [Flags]
    public enum ModelCode : long
    {
        IDOBJ                       = 0x1000000000000000,
        IDOBJ_GID                   = 0x1000000000000104,
        IDOBJ_NAME                  = 0x1000000000000207,
        IDOBJ_DESCRIPTION           = 0x1000000000000307,
        IDOBJ_MRID                  = 0x1000000000000407,

        PSR                         = 0x1100000000000000,
        PSR_MEASUREMENTS            = 0x1100000000000119,

        CONNECTIVITYNODE                            = 0x1200000000010000,
        CONNECTIVITYNODE_CONNECTIVITYNODECONTAINER  = 0x1200000000010109,
        CONNECTIVITYNODE_TERMINALS                  = 0x1200000000010219,

        TERMINAL                    = 0x1300000000020000,
        TERMINAL_CONNECTIVITYNODE   = 0x1300000000020109,
        TERMINAL_CONDUCTINGEQ       = 0x1300000000020209,
        TERMINAL_MEASUREMENTS       = 0x1300000000020319,

        GEOGRAPHICALREGION              = 0x1400000000040000,
        GEOGRAPHICALREGION_SUBREGIONS   = 0x1400000000040119,

        SUBGEOGRAPHICALREGION               = 0x1500000000050000,
        SUBGEOGRAPHICALREGION_SUBSTATIONS   = 0x1500000000050119,
        SUBGEOGRAPHICALREGION_REGION        = 0x1500000000050209,

        MEASUREMENT                 = 0x1600000000000000,
        MEASUREMENT_PSR             = 0x1600000000000109,
        MEASUREMENT_TERMINAL        = 0x1600000000000209,
        MEASUREMENT_ADDRESS         = 0x1600000000000303,
        MEASUREMENT_DIRECTION       = 0x160000000000040A,
        MEASUREMENT_MEASUREMENTYPE  = 0x160000000000050A,

        MEASUREMENTDISCRETE             = 0x1610000000060000,
        MEASUREMENTDISCRETE_MAXVALUE    = 0x1610000000060103,
        MEASUREMENTDISCRETE_MINVALUE    = 0x1610000000060203,
        MEASUREMENTDISCRETE_CURRENTVALUE= 0x1610000000060303,
        MEASUREMENTDISCRETE_DOM         = 0x1610000000060403,

        MEASUREMENTANALOG               = 0x1620000000070000,
        MEASUREMENTANALOG_MINVALUE      = 0x1620000000070105,
        MEASUREMENTANALOG_MAXVALUE      = 0x1620000000070205,
        MEASUREMENTANALOG_CURRENTVALUE  = 0x1620000000070305,

        CONNECTIVIRYNODECONTAINER                   = 0x1110000000000000,
        CONNECTIVIRYNODECONTAINER_CONNECTIVITYNODES = 0x1110000000000119,

        EQUIPMENT                   = 0x1120000000000000,
        EQUIPMENT_EQCONTAINER       = 0x1120000000000109,

        CONDUCTINGEQ                = 0x1121000000000000,
        CONDUCTINGEQ_TERMINALS      = 0x1121000000000119,
        CONDUCTINGEQ_TERMINALS_TEMP = 0x1121000000000119,

        ENERGYCONSUMER              = 0x11212000000B0000,
        ENERGYCONSUMER_PFIXED       = 0x11212000000B0105,
        ENERGYCONSUMER_TYPE         = 0x11212000000B020A,

        ENERGYSOURCE                = 0x11213000000C0000,
        ENERGYSOURCE_ACTIVEPOWER    = 0x11213000000C0105,

        CONDUCTOR                   = 0x1121400000000000,

        ACLINESEG                   = 0x11214100000D0000,

        SWITCH                      = 0x1121500000000000,
        SWITCH_NORMALOPEN           = 0x1121500000000101,

        PROTSWITCH                  = 0x1121510000000000,

        BREAKER                     = 0x11215110000E0000,

        DER                         = 0x1121100000000000,
        DER_SETPOINT                = 0x1121100000000105,
        DER_ACTIVEPOWER             = 0x1121100000000205,
        DER_NOMINALPOWER            = 0x1121100000000305,

        ENERGYSTORAGE                = 0x1121110000080000,
        ENERGYSTORAGE_CAPACITY       = 0x1121110000080105,
        ENERGYSTORAGE_STATEOFCHARGE  = 0x1121110000080205,
        ENERGYSTORAGE_STATE          = 0x112111000008030A,
        ENERGYSTORAGE_GENERATOR      = 0x1121110000080409,

        GENERATOR                = 0x1121120000000000,
        GENERATOR_DELTAPOWER     = 0x1121120000000105,
        GENERATOR_ENERGYSTORAGE  = 0x1121120000000209,

        SOLARGENERATOR           = 0x1121121000090000,

        WINDGENERATOR                = 0x11211210000A0000,
        WINDGENERATOR_CUTOUTSPEED    = 0x11211210000A0105,
        WINDGENERATOR_STARTUPSPEED     = 0x11211210000A0205,
        WINDGENERATOR_NOMINALSPEED   = 0x11211210000A0305,

        EQUIPMENTCONTAINER              = 0x1111000000000000,
        EQUIPMENTCONTAINER_EQUIPEMENTS  = 0x1111000000000119,

        SUBSTATION                  = 0x1111100000030000,
        SUBSTATION_REGION           = 0x1111100000030109,
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
