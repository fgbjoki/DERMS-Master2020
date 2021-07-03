using Core.Common.FEP.CommandingService;
using System.ServiceModel;

namespace Core.Common.ServiceInterfaces.FEP.CommandingService
{
    [ServiceContract]
    public interface ICommanding
    {
        [OperationContract]
        bool SendCommand(Command command);
    }
}
