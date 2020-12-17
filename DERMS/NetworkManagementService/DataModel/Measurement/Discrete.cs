using Common.AbstractModel;
using Common.GDA;

namespace NetworkManagementService.DataModel.Measurement
{
    public class Discrete : Measurement
    {
        public Discrete(long globalId) : base(globalId)
        {

        }

        protected Discrete(Discrete copyObject) : base(copyObject)
        {
            MaxValue = copyObject.MaxValue;
            MinValue = copyObject.MinValue;
            NormalOpen = copyObject.NormalOpen;
            CurrentOpen = copyObject.CurrentOpen;
            Type = copyObject.Type;
        }

        public int MinValue { get; set; }
               
        public int MaxValue { get; set; }

        public bool CurrentOpen { get; set; }

        public bool NormalOpen { get; set; }

        public DiscreteType Type { get; set; }

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
                case ModelCode.MEASUREMENTDISCRETE_CURRENTOPEN:
                    CurrentOpen = property.AsBool();
                    break;
                case ModelCode.MEASUREMENTDISCRETE_NORMALOPEN:
                    NormalOpen = property.AsBool();
                    break;
                case ModelCode.MEASUREMENTDISCRETE_TYPE:
                    Type = (DiscreteType)property.AsEnum();
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
                case ModelCode.MEASUREMENTDISCRETE_CURRENTOPEN:
                case ModelCode.MEASUREMENTDISCRETE_NORMALOPEN:
                case ModelCode.MEASUREMENTDISCRETE_TYPE:
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
                case ModelCode.MEASUREMENTDISCRETE_CURRENTOPEN:
                    property.SetValue(CurrentOpen);
                    break;
                case ModelCode.MEASUREMENTDISCRETE_NORMALOPEN:
                    property.SetValue(NormalOpen);
                    break;
                case ModelCode.MEASUREMENTDISCRETE_TYPE:
                    property.SetValue((short)Type);
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
                && CurrentOpen == compareObject.CurrentOpen && NormalOpen == compareObject.NormalOpen && Type == compareObject.Type && base.Equals(x);
        }

        public override object Clone()
        {
            return new Discrete(this);
        }
    }
}
