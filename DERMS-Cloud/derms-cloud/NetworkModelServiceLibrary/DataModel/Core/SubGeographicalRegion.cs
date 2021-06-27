using System.Collections.Generic;
using Core.Common.AbstractModel;
using Core.Common.GDA;

namespace NetworkManagementService.DataModel.Core
{
    public class SubGeographicalRegion : IdentifiedObject
    {
        public SubGeographicalRegion(long globalId) : base(globalId)
        {
            Substations = new List<long>(1);
        }

        protected SubGeographicalRegion(SubGeographicalRegion copyObject) : base(copyObject)
        {
            Substations = copyObject.Substations;
            GeographicalRegion = copyObject.GeographicalRegion;
        }

        public List<long> Substations { get; set; }

        public long GeographicalRegion { get; set; }

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.SUBGEOGRAPHICALREGION_REGION:
                case ModelCode.SUBGEOGRAPHICALREGION_SUBSTATIONS:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.SUBGEOGRAPHICALREGION_REGION:
                    property.SetValue(GeographicalRegion);
                    break;
                case ModelCode.SUBGEOGRAPHICALREGION_SUBSTATIONS:
                    property.SetValue(Substations);
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
                case ModelCode.SUBGEOGRAPHICALREGION_REGION:
                    GeographicalRegion = property.AsReference();
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
                return Substations?.Count > 0 || base.IsReferenced;
            }
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.SUBSTATION_REGION:
                    Substations.Add(globalId);
                    break;
                default:
                    base.AddReference(referenceId, globalId);
                    break;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (Substations?.Count > 0 && (refType == TypeOfReference.Both || refType == TypeOfReference.Target))
            {
                references[ModelCode.SUBGEOGRAPHICALREGION_SUBSTATIONS] = new List<long>(Substations);
            }

            if (GeographicalRegion > 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.SUBGEOGRAPHICALREGION_REGION] = new List<long>(1) { GeographicalRegion };
            }

            base.GetReferences(references, refType);
        }

        public override void RemoveReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.SUBSTATION_REGION:
                    if (!Substations.Remove(globalId))
                    {
                        // LOG
                        //CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Entity (GID = 0x{0:x16}) doesn't contain reference 0x{1:x16}.", this.GlobalId, globalId);
                    }
                    break;
                default:
                    base.AddReference(referenceId, globalId);
                    break;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object x)
        {
            SubGeographicalRegion compareObject = x as SubGeographicalRegion;

            return compareObject != null && GeographicalRegion == compareObject.GeographicalRegion && CompareHelper.CompareLists(Substations, compareObject.Substations)
                && base.Equals(x);
        }

        public override object Clone()
        {
            return new SubGeographicalRegion(this);
        }
    }
}
