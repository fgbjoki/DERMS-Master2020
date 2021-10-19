using Common.AbstractModel;
using Common.GDA;
using NetworkManagementService.DataModel.Core;

namespace NetworkManagementService.DataModel.Wires
{
    public class EnergyConsumer : ConductingEquipment
    {
        public EnergyConsumer(long globalId) : base(globalId)
        {

        }

        protected EnergyConsumer(EnergyConsumer copyObject) : base(copyObject)
        {
            PFixed = copyObject.PFixed;
            Type = copyObject.Type;
        }

        public float PFixed { get; set; }

        public ConsumerType Type { get; set; }

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.ENERGYCONSUMER_PFIXED:
                case ModelCode.ENERGYCONSUMER_TYPE:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.ENERGYCONSUMER_PFIXED:
                    property.SetValue(PFixed);
                    break;
                case ModelCode.ENERGYCONSUMER_TYPE:
                    property.SetValue((short)Type);
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
                case ModelCode.ENERGYCONSUMER_PFIXED:
                    PFixed = property.AsFloat();
                    break;
                case ModelCode.ENERGYCONSUMER_TYPE:
                    Type = (ConsumerType)property.AsEnum();
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
            EnergyConsumer compareObject = x as EnergyConsumer;

            return compareObject != null && PFixed == compareObject.PFixed && Type == compareObject.Type && base.Equals(x);
        }

        public override object Clone()
        {
            return new EnergyConsumer(this);
        }
    }
}
