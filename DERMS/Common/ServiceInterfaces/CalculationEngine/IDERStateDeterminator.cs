using System.ServiceModel;

namespace Common.ServiceInterfaces.CalculationEngine
{
    [ServiceContract]
    public interface IDERStateDeterminator
    {
        [OperationContract]
        bool IsEntityEnergized(long entityGid);
    }
}