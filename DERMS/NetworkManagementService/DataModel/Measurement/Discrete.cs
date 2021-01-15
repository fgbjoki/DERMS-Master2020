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
        }

        public int MinValue { get; set; }
               
        public int MaxValue { get; set; }

        public int CurrentValue { get; set; }

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
