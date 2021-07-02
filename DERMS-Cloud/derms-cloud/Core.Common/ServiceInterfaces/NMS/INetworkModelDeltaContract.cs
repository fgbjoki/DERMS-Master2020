using Core.Common.GDA;
using System.ServiceModel;

namespace Core.Common.ServiceInterfaces.NMS
{
    [ServiceContract]
    public interface INetworkModelDeltaContract
    {
        /// <summary>
        /// Updates model by appluing reosoreces sent in delta
        /// </summary>		
        /// <param name="delta">Object which contains model changes</param>		
        /// <returns>Result of model changes</returns>
        [OperationContract]
        UpdateResult ApplyUpdate(Delta delta);
    }
}
