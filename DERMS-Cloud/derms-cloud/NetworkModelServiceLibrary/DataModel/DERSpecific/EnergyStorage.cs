using Core.Common.AbstractModel;
using System;
using System.Collections.Generic;
using Core.Common.GDA;
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
                case ModelCode.ENERGYSTORAGE_CAPACITY:
                case ModelCode.ENERGYSTORAGE_STATEOFCHARGE:
                case ModelCode.ENERGYSTORAGE_STATE:
                case ModelCode.ENERGYSTORAGE_GENERATOR:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.ENERGYSTORAGE_CAPACITY:
                    property.SetValue(Capacity);
                    break;
                case ModelCode.ENERGYSTORAGE_STATEOFCHARGE:
                    property.SetValue(StateOfCharge);
                    break;
                case ModelCode.ENERGYSTORAGE_STATE:
                    property.SetValue((short)State);
                    break;
                case ModelCode.ENERGYSTORAGE_GENERATOR:
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
                case ModelCode.ENERGYSTORAGE_CAPACITY:
                    Capacity = property.AsFloat();
                    break;
                case ModelCode.ENERGYSTORAGE_STATEOFCHARGE:
                    StateOfCharge = property.AsFloat();
                    break;
                case ModelCode.ENERGYSTORAGE_STATE:
                    State = (EnergyStorageState)property.AsEnum();
                    break;
                case ModelCode.ENERGYSTORAGE_GENERATOR:
                    Generator = property.AsReference();
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
                return Generator > 0 || base.IsReferenced;
            }
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.GENERATOR_ENERGYSTORAGE:
                    if (Generator == 0)
                    {
                        Generator = globalId;
                    }
                    else
                    {
                        throw new Exception(string.Format("Energy storage {0x16} already has Generator reference!", GlobalId));
                    }
                    break;
                default:
                    base.AddReference(referenceId, globalId);
                    break;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (Generator > 0 && (refType == TypeOfReference.Both || refType == TypeOfReference.Reference))
            {
                references[ModelCode.ENERGYSTORAGE_GENERATOR] = new List<long>(1) { Generator };
            }

            base.GetReferences(references, refType);
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
