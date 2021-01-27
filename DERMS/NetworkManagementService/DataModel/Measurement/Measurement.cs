using Common.AbstractModel;
using NetworkManagementService.DataModel.Core;
using System.Collections.Generic;
using Common.GDA;

namespace NetworkManagementService.DataModel.Measurement
{
    public class Measurement : IdentifiedObject
    {
        public Measurement(long globalId) : base(globalId)
        {

        }

        protected Measurement(Measurement copy) : base(copy)
        {
            MeasurementAddress = copy.MeasurementAddress;
            Direction = copy.Direction;
            MeasurementType = copy.MeasurementType;
            Terminal = copy.Terminal;
            PowerSystemResource = copy.PowerSystemResource;
        }

        public long Terminal { get; set; }

        public long PowerSystemResource { get; set; }

        public int MeasurementAddress { get; set; }

        public SignalDirection Direction { get; set; }

        public MeasurementType MeasurementType { get; set; }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (refType == TypeOfReference.Reference || refType == TypeOfReference.Reference)
            {
                if (Terminal != 0)
                {
                    references[ModelCode.MEASUREMENT_TERMINAL] = new List<long>() { Terminal };
                }

                if (PowerSystemResource != 0)
                {
                    references[ModelCode.MEASUREMENT_PSR] = new List<long>() { PowerSystemResource };
                }
            }

            base.GetReferences(references, refType);
        }

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.MEASUREMENT_TERMINAL:
                case ModelCode.MEASUREMENT_PSR:
                case ModelCode.MEASUREMENT_MEASUREMENTYPE:
                case ModelCode.MEASUREMENT_DIRECTION:
                case ModelCode.MEASUREMENT_ADDRESS:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.MEASUREMENT_PSR:
                    property.SetValue(PowerSystemResource);
                    break;
                case ModelCode.MEASUREMENT_TERMINAL:
                    property.SetValue(Terminal);
                    break;
                case ModelCode.MEASUREMENT_ADDRESS:
                    property.SetValue(MeasurementAddress);
                    break;
                case ModelCode.MEASUREMENT_DIRECTION:
                    property.SetValue((short)Direction);
                    break;
                case ModelCode.MEASUREMENT_MEASUREMENTYPE:
                    property.SetValue((short)MeasurementType);
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
                case ModelCode.MEASUREMENT_PSR:
                    PowerSystemResource = property.AsReference();
                    break;
                case ModelCode.MEASUREMENT_TERMINAL:
                    Terminal = property.AsReference();
                    break;
                case ModelCode.MEASUREMENT_ADDRESS:
                    MeasurementAddress = property.AsInt();
                    break;
                case ModelCode.MEASUREMENT_DIRECTION:
                    Direction = (SignalDirection)property.AsEnum();
                    break;
                case ModelCode.MEASUREMENT_MEASUREMENTYPE:
                    MeasurementType = (MeasurementType)property.AsEnum();
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
            Measurement compareObject = x as Measurement;

            if (compareObject == null)
                return false;

            return Terminal == compareObject.Terminal && PowerSystemResource == compareObject.PowerSystemResource
                && MeasurementAddress == compareObject.MeasurementAddress && MeasurementType == compareObject.MeasurementType
                && Direction == compareObject.Direction && base.Equals(x);
        }

        public override object Clone()
        {
            return new Measurement(this);
        }
    }
}
