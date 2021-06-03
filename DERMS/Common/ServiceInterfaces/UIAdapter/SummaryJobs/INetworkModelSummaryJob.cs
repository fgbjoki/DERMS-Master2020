using System.Collections.Generic;
using Common.UIDataTransferObject.NetworkModel;
using System.ServiceModel;
using Common.AbstractModel;

namespace Common.ServiceInterfaces.UIAdapter.SummaryJobs
{
    [ServiceContract]
    public interface INetworkModelSummaryJob
    {
        [OperationContract]
        List<NetworkModelEntityDTO> GetAllEntities();

        [OperationContract(Name ="GetAllEntitiesWithFilter")]
        List<NetworkModelEntityDTO> GetAllEntities(List<DMSType> entityTypes);

        [OperationContract]
        NetworkModelEntityDTO GetEntity(long globalId);
    }
}