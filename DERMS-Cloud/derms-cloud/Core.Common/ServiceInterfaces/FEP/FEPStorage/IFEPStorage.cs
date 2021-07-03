using Core.Common.Transaction.Models.FEP.FEPStorage;
using System.ServiceModel;

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
    }
}
