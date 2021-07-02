using Core.Common.Transaction.StorageTransactionProcessor;
using System.Collections.Generic;

namespace Core.Common.Transaction.Storage
{
    public interface ITransactionStorage
    {
        List<IStorageTransactionProcessor> GetStorageTransactionProcessors();
    }
}