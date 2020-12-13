using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.AbstractModel;
using Common.GDA;

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

        public float CutInSpeed { get; set; }

        public float CutOutSpeed { get; set; }

        public float NominalSpeed { get; set; }

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.DERWINDGENERATOR_CUTINSPEED:
                case ModelCode.DERWINDGENERATOR_CUTOUTSPEED:
                case ModelCode.DERWINDGENERATOR_NOMINALSPEED:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.DERWINDGENERATOR_CUTINSPEED:
                    property.SetValue(CutInSpeed);
                    break;
                case ModelCode.DERWINDGENERATOR_CUTOUTSPEED:
                    property.SetValue(CutOutSpeed);
                    break;
                case ModelCode.DERWINDGENERATOR_NOMINALSPEED:
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
                case ModelCode.DERWINDGENERATOR_CUTINSPEED:
                    CutInSpeed = property.AsFloat();
                    break;
                case ModelCode.DERWINDGENERATOR_CUTOUTSPEED:
                    CutOutSpeed = property.AsFloat();
                    break;
                case ModelCode.DERWINDGENERATOR_NOMINALSPEED:
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

            return compareObject != null && CutInSpeed == compareObject.CutInSpeed && CutOutSpeed == compareObject.CutOutSpeed
                && NominalSpeed == compareObject.NominalSpeed && base.Equals(x);
        }

        public override object Clone()
        {
            return new WindGenerator(this);
        }
    }
}
