using Common.AbstractModel;
using System.Collections.Generic;

namespace Common.TransactionProcessing.Storage.Helpers
{
    public class AnalogEntityMapper: IAnalogEntityMapper
    {
        private Dictionary<long, AnalogValueOwner> analogValueOwners;

        public AnalogEntityMapper()
        {
            analogValueOwners = new Dictionary<long, AnalogValueOwner>();
        }

        public bool AddAnalogOwner(long measurementGid, MeasurementType measurementType, long ownerGid)
        {
            if (analogValueOwners.ContainsKey(measurementGid))
            {
                return false;
            }

            AnalogValueOwner owner = new AnalogValueOwner(ownerGid, measurementType);

            analogValueOwners.Add(measurementGid, owner);

            return true;
        }

        public AnalogValueOwner GetOwner(long measurementGid)
        {
            AnalogValueOwner owner;

            analogValueOwners.TryGetValue(measurementGid, out owner);

            return owner;
        }
    }
}
