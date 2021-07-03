using Core.Common.FEP.ModbusMessages;
using System.ServiceModel;

namespace Core.Common.ServiceInterfaces.FEP.MessageAggregator
{
    [ServiceContract]
    public interface ICommandSender
    {
        [OperationContract]
        bool SendCommand(ModbusMessageHeader command);
    }
}
