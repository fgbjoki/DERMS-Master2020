﻿using Core.Common.AbstractModel;
using Core.Common.GDA;
using System.Runtime.Serialization;

namespace NetworkManagementService.DataModel.Measurement
{
    [DataContract]
    public class Discrete : Measurement
    {
        public Discrete(long globalId) : base(globalId)
        {

        }

        protected Discrete(Discrete copyObject) : base(copyObject)
        {
            MaxValue = copyObject.MaxValue;
            MinValue = copyObject.MinValue;
        }

        [DataMember]
        public int MinValue { get; set; }

        [DataMember]
        public int MaxValue { get; set; }

        [DataMember]
        public int CurrentValue { get; set; }

        public int DOM { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.MEASUREMENTDISCRETE_MAXVALUE:
                    MaxValue = property.AsInt();
                    break;
                case ModelCode.MEASUREMENTDISCRETE_MINVALUE:
                    MinValue = property.AsInt();
                    break;
                case ModelCode.MEASUREMENTDISCRETE_CURRENTVALUE:
                    CurrentValue = property.AsInt();
                    break;
                case ModelCode.MEASUREMENTDISCRETE_DOM:
                    DOM = property.AsInt();
                    break;
                default:
                    base.SetProperty(property);
                    break;
            }
        }

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.MEASUREMENTDISCRETE_MAXVALUE:
                case ModelCode.MEASUREMENTDISCRETE_MINVALUE:
                case ModelCode.MEASUREMENTDISCRETE_CURRENTVALUE:
                case ModelCode.MEASUREMENTDISCRETE_DOM:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {               
                case ModelCode.MEASUREMENTDISCRETE_MAXVALUE:
                    property.SetValue(MaxValue);
                    break;
                case ModelCode.MEASUREMENTDISCRETE_MINVALUE:
                    property.SetValue(MinValue);
                    break;
                case ModelCode.MEASUREMENTDISCRETE_CURRENTVALUE:
                    property.SetValue(CurrentValue);
                    break;
                case ModelCode.MEASUREMENTDISCRETE_DOM:
                    property.SetValue(DOM);
                    break;
                default:
                    base.GetProperty(property);
                    break;
            }
        }

        public override bool Equals(object x)
        {
            Discrete compareObject = x as Discrete;

            if (compareObject == null)
            {
                return false;
            }

            return  MinValue == compareObject.MinValue && MaxValue == compareObject.MaxValue
                && CurrentValue == compareObject.CurrentValue && base.Equals(x);
        }

        public override object Clone()
        {
            return new Discrete(this);
        }
    }
}
