using Common.AbstractModel;

namespace UIAdapter.TransactionProcessing.Storages.Helpers
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

    public interface IAnalogEntityMapper
    {
        bool AddAnalogOwner(long measurementGid, MeasurementType measurementType, long ownerGid);
        AnalogValueOwner GetOwner(long measurementGid);
    }
}
