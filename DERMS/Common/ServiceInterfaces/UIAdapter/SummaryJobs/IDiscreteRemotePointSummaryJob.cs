using Common.UIDataTransferObject.RemotePoints;
using System.Collections.Generic;
using System.ServiceModel;

namespace Common.ServiceInterfaces.UIAdapter.SummaryJobs
{
    [ServiceContract]
    public interface IDiscreteRemotePointSummaryJob
    {
        [OperationContract]
        List<DiscreteRemotePointSummaryDTO> GetAllDiscreteEntities();
    }
}
