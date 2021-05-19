using System.Collections.Generic;
using Common.UIDataTransferObject.NetworkModel;
using System.ServiceModel;

namespace Common.ServiceInterfaces.UIAdapter.SummaryJobs
{
    [ServiceContract]
    public interface INetworkModelSummaryJob
    {
        [OperationContract]
        List<NetworkModelEntityDTO> GetAllEntities();

        [OperationContract]
        NetworkModelEntityDTO GetEntity(long globalId);
    }
}