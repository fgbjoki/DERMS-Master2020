using System.ServiceModel;

namespace Common.ServiceInterfaces.NetworkDynamicsService.Commands
{
    [ServiceContract]
    public interface INDSCommanding
    {
        [OperationContract]
        bool SendCommand(BaseCommand command);
    }
}
