﻿using FieldSimulator.PowerSimulator.Model.Measurements;
using Common.AbstractModel;
using FTN.ESI.SIMES.CIM.CIMAdapter.Importer;

namespace FieldSimulator.PowerSimulator.Model.Creators
{
    public class DiscreteMeasurementCreator : MeasurementCreator<DERMS.Discrete, DiscreteMeasurement>
    {
        public DiscreteMeasurementCreator(ImportHelper importHelper) : base(DMSType.MEASUREMENTDISCRETE, importHelper)
        {
        }

        protected override DiscreteMeasurement InstantiateNewEntity(long globalId)
        {
            return new DiscreteMeasurement(globalId);
        }
    }
}
