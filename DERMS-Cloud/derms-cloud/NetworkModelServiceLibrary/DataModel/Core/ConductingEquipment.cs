using System.Collections.Generic;
using Core.Common.AbstractModel;
using Core.Common.GDA;

namespace NetworkManagementService.DataModel.Core
{
    public class ConductingEquipment : Equipment
    {
        public ConductingEquipment(long globalId) : base(globalId)
        {

        }

        protected ConductingEquipment(ConductingEquipment copyObject) : base(copyObject)
        {
            Terminals = copyObject.Terminals;
        }

        public List<long> Terminals { get; set; } = new List<long>();

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.CONDUCTINGEQ_TERMINALS:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.CONDUCTINGEQ_TERMINALS:
                    property.SetValue(Terminals);
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
                return Terminals?.Count > 0 || base.IsReferenced;
            }
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.TERMINAL_CONDUCTINGEQ:
                    Terminals.Add(globalId);
                    break;
                default:
                    base.AddReference(referenceId, globalId);
                    break;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (Terminals != null && Terminals.Count > 0 && (refType == TypeOfReference.Both || refType == TypeOfReference.Target))
            {
                references[ModelCode.CONDUCTINGEQ_TERMINALS] = new List<long>(Terminals);
            }

            base.GetReferences(references, refType);
        }

        public override void RemoveReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.TERMINAL_CONDUCTINGEQ:
                    if (!Terminals.Remove(globalId))
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
            ConductingEquipment compareObject = x as ConductingEquipment;

            return compareObject != null && CompareHelper.CompareLists(Terminals, compareObject.Terminals) && base.Equals(x);
        }

        public override object Clone()
        {
            return new ConductingEquipment(this);
        }
    }
}
