using Core.Common.AbstractModel;
using Core.Common.GDA;

namespace NetworkManagementService.DataModel.DER_Specific
{
    public class WindGenerator : Generator
    {
        public WindGenerator(long globalId) : base(globalId)
        {

        }

        protected WindGenerator(WindGenerator copyObject) : base(copyObject)
        {

        }

        public float StartUpSpeed { get; set; }

        public float CutOutSpeed { get; set; }

        public float NominalSpeed { get; set; }

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.WINDGENERATOR_STARTUPSPEED:
                case ModelCode.WINDGENERATOR_CUTOUTSPEED:
                case ModelCode.WINDGENERATOR_NOMINALSPEED:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.WINDGENERATOR_STARTUPSPEED:
                    property.SetValue(StartUpSpeed);
                    break;
                case ModelCode.WINDGENERATOR_CUTOUTSPEED:
                    property.SetValue(CutOutSpeed);
                    break;
                case ModelCode.WINDGENERATOR_NOMINALSPEED:
                    property.SetValue(NominalSpeed);
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
                case ModelCode.WINDGENERATOR_STARTUPSPEED:
                    StartUpSpeed = property.AsFloat();
                    break;
                case ModelCode.WINDGENERATOR_CUTOUTSPEED:
                    CutOutSpeed = property.AsFloat();
                    break;
                case ModelCode.WINDGENERATOR_NOMINALSPEED:
                    NominalSpeed = property.AsFloat();
                    break;
                default:
                    base.SetProperty(property);
                    break;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object x)
        {
            WindGenerator compareObject = x as WindGenerator;

            return compareObject != null && StartUpSpeed == compareObject.StartUpSpeed && CutOutSpeed == compareObject.CutOutSpeed
                && NominalSpeed == compareObject.NominalSpeed && base.Equals(x);
        }

        public override object Clone()
        {
            return new WindGenerator(this);
        }
    }
}
