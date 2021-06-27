using System.Collections.Generic;
using Core.Common.AbstractModel;
using Core.Common.GDA;

namespace NetworkManagementService.DataModel.Core
{
    public class Equipment : PowerSystemResource
    {
        public Equipment(long globalId) : base(globalId)
        {

        }

        protected Equipment(Equipment copyObject) : base(copyObject)
        {
            EquipmentContainer = copyObject.EquipmentContainer;
        }

        public long EquipmentContainer { get; set; }

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.EQUIPMENT_EQCONTAINER:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.EQUIPMENT_EQCONTAINER:
                    property.SetValue(EquipmentContainer);
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
                case ModelCode.EQUIPMENT_EQCONTAINER:
                    EquipmentContainer = property.AsReference();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (EquipmentContainer > 0 && (refType == TypeOfReference.Both || refType == TypeOfReference.Reference))
            {
                references[ModelCode.EQUIPMENT_EQCONTAINER] = new List<long>(1) { EquipmentContainer };
            }

            base.GetReferences(references, refType);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object x)
        {
            Equipment compareObject = x as Equipment;

            return compareObject != null && EquipmentContainer == compareObject.EquipmentContainer && base.Equals(x);
        }

        public override object Clone()
        {
            return new Equipment(this);
        }
    }
}
