using System.Collections.Generic;
using Core.Common.AbstractModel;
using Core.Common.GDA;
using NetworkManagementService.DataModel.Core;

namespace NetworkManagementService.DataModel.DER_Specific
{
    public class Generator : DistributedEnergyResource
    {
        public Generator(long globalId) : base(globalId)
        {

        }

        protected Generator(Generator copyObject) : base(copyObject)
        {
            DeltaPower = copyObject.DeltaPower;
        }

        public float DeltaPower { get; set; }

        public long Storage { get; set; }

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.GENERATOR_DELTAPOWER:
                case ModelCode.GENERATOR_ENERGYSTORAGE:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.GENERATOR_DELTAPOWER:
                    property.SetValue(DeltaPower);
                    break;
                case ModelCode.GENERATOR_ENERGYSTORAGE:
                    property.SetValue(Storage);
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
                case ModelCode.GENERATOR_DELTAPOWER:
                    DeltaPower = property.AsFloat();
                    break;
                case ModelCode.GENERATOR_ENERGYSTORAGE:
                    Storage = property.AsReference();
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
                return Storage > 0 || base.IsReferenced;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (Storage > 0 && (refType == TypeOfReference.Both || refType == TypeOfReference.Reference))
            {
                references[ModelCode.GENERATOR_ENERGYSTORAGE] = new List<long>(1) { Storage };
            }

            base.GetReferences(references, refType);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object x)
        {
            Generator compareObject = x as Generator;

            return compareObject != null && Storage == compareObject.Storage && DeltaPower == compareObject.DeltaPower;
        }

        public override object Clone()
        {
            return new Generator(this);
        }

    }
}
