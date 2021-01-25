using FieldProcessor.ModbusMessages;

namespace FieldProcessor.MessageValidation
{
    interface IResponseCommandCreator
    {
        ModbusMessageHeader CreateResponse(IRequestMessage request, byte[] rawResponse);
    }
}
