using FieldProcessor.ModbusMessages;

namespace FieldProcessor.MessageValidation
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
