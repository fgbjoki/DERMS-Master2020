using System.ServiceModel;

namespace Common.SCADA.FieldProcessor
{
    [ServiceContract]
    public interface ICommanding
    {
        [OperationContract]
        bool SendCommand(Command command);
    }
}
