using NetworkManagementService.DataModel.Core;
using Common.AbstractModel;
using Common.GDA;

namespace NetworkManagementService.DataModel.Wires
{
    public class EnergySource : ConductingEquipment
    {
        public EnergySource(long globalId) : base(globalId)
        {

        }

        protected EnergySource(EnergySource copyObject) : base(copyObject)
        {
            ActivePower = copyObject.ActivePower;
        }

        public float ActivePower { get; set; }

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.ENERGYSOURCE_ACTIVEPOWER:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.ENERGYSOURCE_ACTIVEPOWER:
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
                case ModelCode.ENERGYSOURCE_ACTIVEPOWER:
                    ActivePower = property.AsFloat();
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
            EnergySource compareObject = x as EnergySource;

            return compareObject != null && ActivePower == compareObject.ActivePower && base.Equals(x);
        }

        public override object Clone()
        {
            return new EnergySource(this);
        }
    }
}
