using Core.Common.AbstractModel;
using Core.Common.GDA;
using NetworkManagementService.DataModel.Core;
using System.Runtime.Serialization;

namespace NetworkManagementService.DataModel.Wires
{
    [DataContract]
    public class Switch : ConductingEquipment
    {
        public Switch(long globalId) : base(globalId)
        {

        }

        protected Switch(Switch copyObject) : base(copyObject)
        {
            NormalOpen = copyObject.NormalOpen;
        }

        [DataMember]
        public bool NormalOpen { get; set; }

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.SWITCH_NORMALOPEN:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.SWITCH_NORMALOPEN:
                    property.SetValue(NormalOpen);
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
                case ModelCode.SWITCH_NORMALOPEN:
                    NormalOpen = property.AsBool();
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
            Switch compareObject = x as Switch;

            return compareObject != null && base.Equals(x) && NormalOpen == compareObject.NormalOpen;
        }
    }
}
