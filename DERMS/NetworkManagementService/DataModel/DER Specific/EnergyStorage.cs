using Common.AbstractModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.GDA;
using NetworkManagementService.DataModel.Core;

namespace NetworkManagementService.DataModel.DER_Specific
{
    public class EnergyStorage : DistributedEnergyResource
    {
        public EnergyStorage(long globalId) : base(globalId)
        {

        }

        protected EnergyStorage(EnergyStorage copyObject) : base(copyObject)
        {
            State = copyObject.State;
            Capacity = copyObject.Capacity;
            Generator = copyObject.Generator;
            StateOfCharge = copyObject.StateOfCharge;
        }

        public EnergyStorageState State { get; set; }

        public float StateOfCharge { get; set; }

        public float Capacity { get; set; }

        public long Generator { get; set; }

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.DERENERGYSTORAGE_CAPACITY:
                case ModelCode.DERENERGYSTORAGE_STATEOFCHARGE:
                case ModelCode.DERENERGYSTORAGE_STATE:
                case ModelCode.DERENERGYSTORAGE_GENERATOR:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.DERENERGYSTORAGE_CAPACITY:
                    property.SetValue(Capacity);
                    break;
                case ModelCode.DERENERGYSTORAGE_STATEOFCHARGE:
                    property.SetValue(StateOfCharge);
                    break;
                case ModelCode.DERENERGYSTORAGE_STATE:
                    property.SetValue((short)State);
                    break;
                case ModelCode.DERENERGYSTORAGE_GENERATOR:
                    property.SetValue(Generator);
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
                case ModelCode.DERENERGYSTORAGE_CAPACITY:
                    Capacity = property.AsFloat();
                    break;
                case ModelCode.DERENERGYSTORAGE_STATEOFCHARGE:
                    StateOfCharge = property.AsFloat();
                    break;
                case ModelCode.DERENERGYSTORAGE_STATE:
                    State = (EnergyStorageState)property.AsFloat();
                    break;
                case ModelCode.DERENERGYSTORAGE_GENERATOR:
                    Generator = property.AsReference();
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
                return Generator > 0 || base.IsReferenced;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (Generator > 0 && (refType == TypeOfReference.Both || refType == TypeOfReference.Reference))
            {
                references[ModelCode.DERENERGYSTORAGE_GENERATOR] = new List<long>(1) { Generator };
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object x)
        {
            EnergyStorage compareObject = x as EnergyStorage;

            return compareObject != null && Generator == compareObject.Generator && State == compareObject.State
                && StateOfCharge == compareObject.StateOfCharge && Capacity == compareObject.Capacity && base.Equals(x);
        }

        public override object Clone()
        {
            return new EnergyStorage(this);
        }
    }
}
