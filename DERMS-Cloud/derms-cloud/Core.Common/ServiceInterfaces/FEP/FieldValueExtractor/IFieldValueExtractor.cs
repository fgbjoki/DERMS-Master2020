using Core.Common.FEP.ModbusMessages;
using System.ServiceModel;

namespace Core.Common.ServiceInterfaces.FEP.FieldValueExtractor
{
    [ServiceContract]
    public interface IFieldValueExtractor
    {
        [OperationContract]
        void ExtractValues(ModbusMessageHeader request, ModbusMessageHeader response);
    }
}
