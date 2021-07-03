using Core.Common.FEP.ModbusMessages;
using Core.Common.FEP.ModbusMessages.RequestMessages;

namespace MessageAggregatorService.MessageAggregator
{
    public class ResponseCommandCreator<T> : IResponseCommandCreator
        where T : ModbusMessageHeader, new()
    {
        public ModbusMessageHeader CreateResponse(IRequestMessage request, byte[] rawResponse)
        {
            T response = new T();
            response.ConvertMessageFromBytes(rawResponse);

            if (!request.ValidateResponse(response))
            {
                return null;
            }

            return response;
        }
    }
}
