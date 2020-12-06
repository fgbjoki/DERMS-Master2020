using System.ServiceModel;

namespace Common.ServiceInterfaces.Transaction
{
    /// <summary>
    /// Used for components which participate in transaction.
    /// </summary>
    [ServiceContract]
    public interface ITransactionCallback
    {
        /// <summary>
        /// Perform prepare phase.
        /// </summary>
        /// <returns><b>True</b> if the prepared phase goes trough, otherwise <b>false</b>.</returns>
        [OperationContract]
        bool Prepare();

        /// <summary>
        /// Perform commit phase.
        /// </summary>
        /// <returns><b>True</b> if the commit phase goes trough, otherwise <b>false</b>.</returns>
        [OperationContract]
        bool Commit();

        /// <summary>
        /// Perform prepare phase.
        /// </summary>
        /// <returns><b>True</b> if the rollback phase goes trough, otherwise <b>false</b>.</returns>
        [OperationContract]
        bool Rollback();
    }
}
