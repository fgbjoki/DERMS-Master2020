﻿using System.ServiceModel;

namespace Core.Common.ServiceInterfaces.Transaction
{
    /// <summary>
    /// Used for components which participate in transaction.
    /// </summary>
    [ServiceContract]
    public interface ITransaction
    {
        /// <summary>
        /// Perform prepare phase.
        /// </summary>
        /// <returns><b>True</b> if the prepared phase goes through, otherwise <b>false</b>.</returns>
        [OperationContract]
        bool Prepare();

        /// <summary>
        /// Perform commit phase.
        /// </summary>
        /// <returns><b>True</b> if the commit phase goes through, otherwise <b>false</b>.</returns>
        [OperationContract]
        bool Commit();

        /// <summary>
        /// Perform rollback phase.
        /// </summary>
        /// <returns><b>True</b> if the rollback phase goes through, otherwise <b>false</b>.</returns>
        [OperationContract]
        bool Rollback();
    }
}
