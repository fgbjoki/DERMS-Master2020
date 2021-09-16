using Common.UIDataTransferObject.EnergyBalanceForecast;
using System.ServiceModel;

namespace Common.ServiceInterfaces.UIAdapter
{
    [ServiceContract]
    public interface IEnergyBalanceForecast
    {
        [OperationContract]
        int Compute(DomainParametersDTO domainParameters);

        [OperationContract]
        DERStatesSuggestionDTO GetResults(int clientId);
    }
}