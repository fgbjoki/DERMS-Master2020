using Common.UIDataTransferObject.RemotePoints;
using System.Collections.Generic;
using System.ServiceModel;

namespace Common.ServiceInterfaces.UIAdapter.SummaryJobs
{
    [ServiceContract]
    public interface IAnalogRemotePointSummaryJob
    {
        [OperationContract]
        List<AnalogRemotePointSummaryDTO> GetAllAnalogEntities();

        [OperationContract]
        AnalogRemotePointSummaryDTO GetEntity(long globalId);
    }
}
