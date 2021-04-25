using Common.AbstractModel;

namespace UIAdapter.TransactionProcessing.Storages
{
    public struct AnalogValueOwner
    {
        public AnalogValueOwner(long ownerGlobalId, MeasurementType measurementType)
        {
            OwnerGlobalId = ownerGlobalId;
            MeasurementType = measurementType;
        }

        public long OwnerGlobalId { get; set; }
        public MeasurementType MeasurementType { get; set; }
    }

    public interface IAnalogEntityStorage
    {
        void UpdateValue(long measurementDiscrete, float newValue);
    }
}
