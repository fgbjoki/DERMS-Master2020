using Core.Common.AbstractModel;
using Core.Common.GDA;

namespace NetworkManagementService.DataModel.Core
{
    public class Substation : EquipmentContainer
    {
        public Substation(long globalId) : base(globalId)
        {

        }

        protected Substation(Substation copyObject) : base(copyObject)
        {
            SubGeographicalRegion = copyObject.SubGeographicalRegion;
        }

        public long SubGeographicalRegion { get; set; }

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.SUBSTATION_REGION:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.SUBSTATION_REGION:
                    property.SetValue(SubGeographicalRegion);
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
                case ModelCode.SUBSTATION_REGION:
                    SubGeographicalRegion = property.AsReference();
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
            Substation compareObject = x as Substation;

            return compareObject != null && SubGeographicalRegion == compareObject.SubGeographicalRegion && base.Equals(x);
        }

        public override object Clone()
        {
            return new Substation(this);
        }
    }
}
