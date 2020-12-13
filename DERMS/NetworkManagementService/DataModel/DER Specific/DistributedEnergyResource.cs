using NetworkManagementService.DataModel.Core;
using Common.AbstractModel;
using Common.GDA;

namespace NetworkManagementService.DataModel.DER_Specific
{
    public class DistributedEnergyResource : ConductingEquipment
    {
        public DistributedEnergyResource(long globalId) : base (globalId)
        {

        }

        protected DistributedEnergyResource(DistributedEnergyResource copyObject) : base(copyObject)
        {
            SetPoint = copyObject.SetPoint;
            ActivePower = copyObject.ActivePower;
            NominalPower = copyObject.NominalPower;
        }

        public float ActivePower { get; set; }

        public float SetPoint { get; set; }

        public float NominalPower { get; set; }

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.DER_SETPOINT:
                case ModelCode.DER_NOMINALPOWER:
                case ModelCode.DER_ACTIVEPOWER:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.DER_SETPOINT:
                    property.SetValue(SetPoint);
                    break;
                case ModelCode.DER_NOMINALPOWER:
                    property.SetValue(NominalPower);
                    break;
                case ModelCode.DER_ACTIVEPOWER:
                    property.SetValue(ActivePower);
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
                case ModelCode.DER_SETPOINT:
                    SetPoint = property.AsFloat();
                    break;
                case ModelCode.DER_NOMINALPOWER:
                    NominalPower = property.AsFloat();
                    break;
                case ModelCode.DER_ACTIVEPOWER:
                    ActivePower = property.AsFloat();
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
            DistributedEnergyResource copyObject = x as DistributedEnergyResource;

            return copyObject != null && NominalPower == copyObject.NominalPower && ActivePower == copyObject.ActivePower && SetPoint == copyObject.SetPoint;
        }

        public override object Clone()
        {
            return new DistributedEnergyResource(this);
        }
    }
}
