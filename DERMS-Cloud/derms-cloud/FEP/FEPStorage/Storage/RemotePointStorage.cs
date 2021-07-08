using Core.Common.AbstractModel;
using Core.Common.ServiceInterfaces.FEP.FEPStorage;
using Core.Common.Transaction.Models.FEP.FEPStorage;
using FEPStorage.Transaction.Storage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FEPStorage.Storage
{
    public class RemotePointStorage : IFEPStorage
    {
        public AnalogRemotePointStorage AnalogStorage { get; set; }
        public DiscreteRemotePointStorage DiscreteStorage { get; set; }

        public Task<List<RemotePoint>> GetEntities(List<DMSType> entityDMSType)
        {
            List<RemotePoint> entities = new List<RemotePoint>();
            if (AnalogStorage != null && entityDMSType.Contains(DMSType.MEASUREMENTANALOG))
            {
                entities.AddRange(AnalogStorage.GetAllEntities());
            }

            if (entityDMSType.Contains(DMSType.MEASUREMENTDISCRETE) && DiscreteStorage != null)
            {
                entities.AddRange(DiscreteStorage?.GetAllEntities());
            }

            return Task.FromResult(entities);
        }

        public RemotePoint GetEntity(long globalId)
        {
            DMSType dmsType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(globalId);

            RemotePoint remotePoint = null;

            if (dmsType == DMSType.MEASUREMENTANALOG && AnalogStorage != null)
            {
                remotePoint = AnalogStorage.GetEntity(globalId);
            }
            else if (dmsType == DMSType.MEASUREMENTDISCRETE && DiscreteStorage != null)
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
