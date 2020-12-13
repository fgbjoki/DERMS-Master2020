﻿using Common.AbstractModel;
using Common.GDA;

namespace NetworkManagementService.DataModel.Measurement
{
    public class Analog : Measurement
    {
        public Analog(long globalId) : base(globalId)
        {

        }

        protected Analog(Analog copyObject) : base(copyObject)
        {
            MinValue = copyObject.MinValue;
            MaxValue = copyObject.MaxValue;
            CurrentValue = copyObject.CurrentValue;
        }

        public float MinValue { get; set; }

        public float MaxValue { get; set; }

        public float CurrentValue { get; set; }

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.MEASUREMENTANALOG_MINVALUE:
                case ModelCode.MEASUREMENTANALOG_MAXVALUE:
                case ModelCode.MEASUREMENTANALOG_CURRENTVALUE:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.MEASUREMENTANALOG_MINVALUE:
                    property.SetValue(MinValue);
                    break;
                case ModelCode.MEASUREMENTANALOG_MAXVALUE:
                    property.SetValue(MaxValue);
                    break;
                case ModelCode.MEASUREMENTANALOG_CURRENTVALUE:
                    property.SetValue(CurrentValue);
                    break;
                default:
                    base.GetProperty(property);
                    break;
            }
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.MEASUREMENTANALOG_MINVALUE:
                    MinValue = property.AsFloat();
                    break;
                case ModelCode.MEASUREMENTANALOG_MAXVALUE:
                    MaxValue = property.AsFloat();
                    break;
                case ModelCode.MEASUREMENTANALOG_CURRENTVALUE:
                    CurrentValue = property.AsFloat();
                    break;
                default:
                    base.GetProperty(property);
                    break;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object x)
        {
            Analog compareObject = x as Analog;
            if (compareObject == null)
            {
                return false;
            }

            return MinValue == compareObject.MinValue && MaxValue == compareObject.MaxValue && CurrentValue == compareObject.MaxValue && base.Equals(x);
        }
        public override object Clone()
        {
            return new Analog(this);
        }
    }
}
