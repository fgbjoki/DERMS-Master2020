using Core.Common.AbstractModel;
using Core.Common.GDA;
using System.Collections.Generic;

namespace NetworkManagementService.DataModel.Core
{
    public class PowerSystemResource : IdentifiedObject
    {
        /// <summary>
        /// Initializes a new instance of the PowerSystemResource class.
        /// </summary>		
        /// <param name="globalId">Global id of the entity.</param>
        public PowerSystemResource(long globalId) : base(globalId)
        {
            Measurements = new List<long>(1);
        }

        /// <summary>
        /// Copy constructor for <see cref="ICloneable"/>.
        /// <remark>This will be used for Shallow Copy durning model promotion.</remark>
        /// </summary>
        /// <param name="powerSystemResource"><see cref="PowerSystemResource"/> to copy.</param>
        protected PowerSystemResource(PowerSystemResource powerSystemResource) : base (powerSystemResource)
        {
            Measurements = powerSystemResource.Measurements;
        }

        /// <summary>
        /// Measurements for given <see cref="PowerSystemResource"/>.
        /// </summary>
        public List<long> Measurements { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object x)
        {
            PowerSystemResource psr = (PowerSystemResource)x;

            if (psr == null)
            {
                return false;
            }

            return CompareHelper.CompareLists(Measurements, psr.Measurements) && base.Equals(x);
        }

        public override bool IsReferenced
        {
            get
            {
                return Measurements.Count != 0 || base.IsReferenced;
            }
        }

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.PSR_MEASUREMENTS:
                    return true;

                default:
                    return false;
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.PSR_MEASUREMENTS:
                    property.SetValue(Measurements);
                    break;

                default:
                    base.GetProperty(property);
                    break;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (Measurements != null && Measurements.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.PSR_MEASUREMENTS] = Measurements.GetRange(0, Measurements.Count);
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.MEASUREMENT_PSR:
                    Measurements.Add(globalId);
                    break;

                default:
                    base.AddReference(referenceId, globalId);
                    break;
            }
        }

        public override void RemoveReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.MEASUREMENT_PSR:

                    if (Measurements.Contains(globalId))
                    {
                        Measurements.Remove(globalId);
                    }
                    else
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

        public override object Clone()
        {
            return new PowerSystemResource(this);
        }
    }
}
