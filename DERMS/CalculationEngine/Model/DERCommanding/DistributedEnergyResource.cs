﻿using Common.ComponentStorage;

namespace CalculationEngine.Model.DERCommanding
{
    public class DistributedEnergyResource : IdentifiedObject
    {
        public DistributedEnergyResource(long globalId) : base(globalId)
        {
        }

        public float NominalPower { get; set; }

        public float ActivePower { get; set; }
        public long ActivePowerMeasurementGid { get; set; }
    }
}
