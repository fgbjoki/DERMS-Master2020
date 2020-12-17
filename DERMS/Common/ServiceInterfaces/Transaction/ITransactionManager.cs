using System.ServiceModel;

namespace Common.ServiceInterfaces.Transaction
{
    /// <summary>
    /// Represents actions which can be preformed on the transaction manager.
    /// </summary>
    [ServiceContract]
    public interface ITransactionManager
    {
        /// <summary>
        /// Starts the enlist procedure.
        /// </summary>
        /// <returns><b>True</b> if the start enlist procedure is performed. If the trasaction is already started, <b>false</b> will be returned.</returns>
        [OperationContract]
        bool StartEnlist();

        /// <summary>
        /// Enlists the service in the ongoing transaction.
        /// </summary>
        /// <param name="serviceName">Name of the service which wants to participate in the transaction.</param>
        /// <param name="serviceEnedPoint">Endpoint of the <paramref="serviceName"/> service.</param>
        /// <returns><b>True</b> if the service is sucessfuly enlisted, otherwise <b>false</b>.</returns>
        [OperationContract]
        bool EnlistService(string serviceName, string serviceEndpoint);

        /// <summary>
        /// Ends the enlist procedure.
        /// </summary>
        /// <param name="allServicesEnlisted"><b>True</b> if all ment services are enlisted, otherwise <b>false</b> which forces transaction into <b>Rollback</b> phase.</param>
        /// <returns><b>True</b> if the procedure is successfuly executed, otherwise <b>false</b>.</returns>
        [OperationContract]
        bool EndEnlist(bool allServicesEnlisted);
    }
}
