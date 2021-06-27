using System.Collections.Generic;
using Core.Common.AbstractModel;
using Core.Common.GDA;

namespace NetworkManagementService.DataModel.Core
{
    public class EquipmentContainer : ConnectivityNodeContainer
    {
        public EquipmentContainer(long globalId) : base(globalId)
        {

        }

        protected EquipmentContainer(EquipmentContainer copyObject) : base(copyObject)
        {
            Equipments = copyObject.Equipments;
        }

        public List<long> Equipments { get; set; } = new List<long>();

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.EQUIPMENTCONTAINER_EQUIPEMENTS:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.EQUIPMENTCONTAINER_EQUIPEMENTS:
                    property.SetValue(Equipments);
                    break;
                default:
                    base.GetProperty(property);
                    break;
            }
        }

        public override bool IsReferenced
        {
            get
            {
                return Equipments?.Count > 0 || base.IsReferenced;
            }
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.EQUIPMENT_EQCONTAINER:
                    Equipments.Add(globalId);
                    break;
                default:
                    base.AddReference(referenceId, globalId);
                    break;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (Equipments?.Count > 0 && (refType == TypeOfReference.Both || refType == TypeOfReference.Target))
            {
                references[ModelCode.EQUIPMENTCONTAINER_EQUIPEMENTS] = new List<long>(Equipments);
            }

            base.GetReferences(references, refType);
        }

        public override void RemoveReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.EQUIPMENT_EQCONTAINER:
                    if (!Equipments.Remove(globalId))
                    {
                        // LOG
                        //CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Entity (GID = 0x{0:x16}) doesn't contain reference 0x{1:x16}.", this.GlobalId, globalId);
                    }
                    break;
                default:
                    base.RemoveReference(referenceId, globalId);
                    break;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object x)
        {
            EquipmentContainer compareObject = x as EquipmentContainer;

            return compareObject != null && CompareHelper.CompareLists(Equipments, compareObject.Equipments) && base.Equals(x);
        }

        public override object Clone()
        {
            return new EquipmentContainer(this);
        }
    }
}
