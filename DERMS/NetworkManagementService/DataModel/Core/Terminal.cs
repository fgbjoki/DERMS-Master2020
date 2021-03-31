using System.Collections.Generic;
using Common.AbstractModel;
using Common.GDA;

namespace NetworkManagementService.DataModel.Core
{
    public class Terminal : IdentifiedObject
    {
        public Terminal(long globalId) : base(globalId)
        {
            Measurements = new List<long>(1);
        }

        protected Terminal(Terminal copyObject) : base(copyObject)
        {
            Measurements = copyObject.Measurements;
            ConnectivityNode = copyObject.ConnectivityNode;
            ConductingEquipment = copyObject.ConductingEquipment;
        }

        public long ConnectivityNode { get; set; }

        public long ConductingEquipment { get; set; }

        public List<long> Measurements { get; set; }

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.TERMINAL_CONDUCTINGEQ:
                case ModelCode.TERMINAL_CONNECTIVITYNODE:
                case ModelCode.TERMINAL_MEASUREMENTS:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.TERMINAL_CONDUCTINGEQ:
                    property.SetValue(ConductingEquipment);
                    break;
                case ModelCode.TERMINAL_CONNECTIVITYNODE:
                    property.SetValue(ConnectivityNode);
                    break;
                case ModelCode.TERMINAL_MEASUREMENTS:
                    property.SetValue(Measurements);
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
                case ModelCode.TERMINAL_CONDUCTINGEQ:
                    ConductingEquipment = property.AsReference();
                    break;
                case ModelCode.TERMINAL_CONNECTIVITYNODE:
                    ConnectivityNode = property.AsReference();
                    break;
                default:
                    base.SetProperty(property);
                    break;
            }
        }

        public override bool IsReferenced
        {
            get
            {
                return ConductingEquipment > 0 || ConnectivityNode > 0 || Measurements?.Count > 0 || base.IsReferenced;
            }
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.MEASUREMENT_TERMINAL:
                    Measurements.Add(globalId);
                    break;
                default:
                    base.AddReference(referenceId, globalId);
                    break;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (refType == TypeOfReference.Reference || refType == TypeOfReference.Both)
            {
                if (ConnectivityNode > 0)
                {
                    references[ModelCode.TERMINAL_CONNECTIVITYNODE] = new List<long>(1) { ConnectivityNode };
                }

                references[ModelCode.TERMINAL_CONDUCTINGEQ] = new List<long>(1) { ConductingEquipment };
            }
            
            if (Measurements?.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.TERMINAL_MEASUREMENTS] = new List<long>(Measurements);
            }

            base.GetReferences(references, refType);
        }

        public override void RemoveReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.MEASUREMENT_TERMINAL:
                    if (!Measurements.Remove(globalId))
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
            Terminal compareObject = x as Terminal;

            return compareObject != null && ConnectivityNode == compareObject.ConnectivityNode && ConductingEquipment == compareObject.ConductingEquipment
                && CompareHelper.CompareLists(Measurements, compareObject.Measurements) && base.Equals(x);
        }

        public override object Clone()
        {
            return new Terminal(this);
        }
    }
}
