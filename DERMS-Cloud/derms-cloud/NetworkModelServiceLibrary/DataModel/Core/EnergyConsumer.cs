using Core.Common.AbstractModel;
using Core.Common.GDA;
using NetworkManagementService.DataModel.Core;
using System.Runtime.Serialization;

namespace NetworkManagementService.DataModel.Wires
{
    [DataContract]
    public class EnergyConsumer : ConductingEquipment
    {
        public EnergyConsumer(long globalId) : base(globalId)
        {

        }

        protected EnergyConsumer(EnergyConsumer copyObject) : base(copyObject)
        {
            PFixed = copyObject.PFixed;
        }

        public float PFixed { get; set; }

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.ENERGYCONSUMER_PFIXED:
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

            return compareObject != null && PFixed == compareObject.PFixed && base.Equals(x);
        }

        public override object Clone()
        {
            return new EnergyConsumer(this);
        }
    }
}
