﻿using Common.ComponentStorage;

namespace CalculationEngine.Model.DERStates
{
    public class DERState : IdentifiedObject
    {
        private float activePower;

        public DERState(long globalId) : base(globalId)
        {
        }

        public float ActivePower
        {
            get
            {
                return !IsEnergized ? 0 : activePower;
            }
            set
            {
                activePower = value;
            }
        }

        public float NominalPower { get; set; }

        public long ActivePowerMeasurementGid { get; set; }

        public bool IsEnergized { get; set; }

        public long ConnectedSourceGid { get; set; }
    }
}
