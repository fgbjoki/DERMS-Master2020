using Core.Common.AbstractModel;
using Core.Common.Transaction.Models.FEP.FEPStorage;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Core.Common.ServiceInterfaces.FEP.FEPStorage
{
    [ServiceContract]
    public interface IFEPStorage
    {
        [OperationContract]
        void UpdateAnalogRemotePointValue(long globalId, float value);

        [OperationContract]
        void UpdateDiscreteRemotePointValue(long globalId, float value);

        [OperationContract]
        RemotePoint GetEntity(long globalId);

        [OperationContract]
        Task<List<RemotePoint>> GetEntities(List<DMSType> entityDMSType);
    }
}
