using Common.SCADA;
using System.Collections.Generic;
using System.ServiceModel;

namespace Common.ServiceInterfaces.NetworkDynamicsService
{
    [ServiceContract]
    public interface IFieldValuesProcessing
    {
        [OperationContract]
        void ProcessFieldValues(IEnumerable<RemotePointFieldValue> fieldValues);
    }
}
