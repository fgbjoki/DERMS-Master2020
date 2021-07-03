using Core.Common.FEP.ModbusMessages;
using Core.Common.ServiceInterfaces.FEP.MessageAggregator;
using System.ServiceModel;

namespace Core.Common.Communication.ServiceFabric.FEP
{
    public class CommandSenderWCFClient : ClientBase<ICommandSender>, ICommandSender
    {
        public CommandSenderWCFClient() : base(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:12122/MessageAggregator/ICommandSender"))
        {
        }

        public bool SendCommand(ModbusMessageHeader command)
        {
            return CreateChannel().SendCommand(command);
        }
    }
}
