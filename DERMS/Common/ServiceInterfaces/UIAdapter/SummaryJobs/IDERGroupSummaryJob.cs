using Common.UIDataTransferObject.DERGroup;
using System.Collections.Generic;
using System.ServiceModel;

namespace Common.ServiceInterfaces.UIAdapter.SummaryJobs
{
    [ServiceContract]
    public interface IDERGroupSummaryJob
    {
        [OperationContract]
        List<DERGroupSummaryDTO> GetAllAnalogEntities();

        [OperationContract]
        DERGroupSummaryDTO GetEntity(long globalId);
    }
}
