using System.Collections.Generic;
using Common.AbstractModel;
using Common.GDA;

namespace NetworkManagementService.DataModel.Core
{
    public class GeographicalRegion : IdentifiedObject
    {
        public GeographicalRegion(long globalId) : base(globalId)
        {

        }

        protected GeographicalRegion(GeographicalRegion copyObject) : base(copyObject)
        {
            SubGeographicalRegions = copyObject.SubGeographicalRegions;
        }

        public List<long> SubGeographicalRegions { get; set; } = new List<long>(1);

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.GEOGRAPHICALREGION_SUBREGIONS:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.GEOGRAPHICALREGION_SUBREGIONS:
                    property.SetValue(SubGeographicalRegions);
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
                return SubGeographicalRegions?.Count > 0 || base.IsReferenced;
            }
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.SUBGEOGRAPHICALREGION_REGION:
                    SubGeographicalRegions.Add(globalId);
                    break;
                default:
                    base.AddReference(referenceId, globalId);
                    break;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (SubGeographicalRegions?.Count > 0 && (refType == TypeOfReference.Both || refType == TypeOfReference.Target))
            {
                references[ModelCode.GEOGRAPHICALREGION_SUBREGIONS] = new List<long>(SubGeographicalRegions);
            }

            base.GetReferences(references, refType);
        }

        public override void RemoveReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.SUBGEOGRAPHICALREGION_REGION:
                    if (!SubGeographicalRegions.Remove(globalId))
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
            GeographicalRegion compareObject = x as GeographicalRegion;

            return compareObject != null && CompareHelper.CompareLists(SubGeographicalRegions, compareObject.SubGeographicalRegions) && base.Equals(x);
        }

        public override object Clone()
        {
            return new GeographicalRegion(this);
        }
    }
}
