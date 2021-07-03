using Core.Common.FEP.ModbusMessages;
using Core.Common.FEP.ModbusMessages.RequestMessages;

namespace MessageAggregatorService.MessageAggregator
{
    interface IResponseCommandCreator
    {
        ModbusMessageHeader CreateResponse(IRequestMessage request, byte[] rawResponse);
    }
}
