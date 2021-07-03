using Core.Common.AbstractModel;
using Core.Common.ServiceInterfaces.FEP.FEPStorage;
using Core.Common.Transaction.Models.FEP.FEPStorage;
using FEPStorage.Transaction.Storage;
using System;

namespace FEPStorage.Storage
{
    public class RemotePointStorage : IFEPStorage
    {
        public AnalogRemotePointStorage AnalogStorage { get; set; }
        public DiscreteRemotePointStorage DiscreteStorage { get; set; }

        public RemotePoint GetEntity(long globalId)
        {
            DMSType dmsType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(globalId);

            RemotePoint remotePoint = null;

            if (dmsType == DMSType.MEASUREMENTANALOG)
            {
                remotePoint = AnalogStorage.GetEntity(globalId);
            }
            else if (dmsType == DMSType.MEASUREMENTDISCRETE)
            {
                remotePoint = DiscreteStorage.GetEntity(globalId);
            }

            return remotePoint;
        }

        public void UpdateAnalogRemotePointValue(long globalId, float value)
        {
            throw new NotImplementedException();
        }

        public void UpdateDiscreteRemotePointValue(long globalId, float value)
        {
            throw new NotImplementedException();
        }
    }
}
