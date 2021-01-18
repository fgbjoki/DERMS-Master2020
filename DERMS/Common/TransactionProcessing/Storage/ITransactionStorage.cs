using Common.AbstractModel;
using System.Collections.Generic;

namespace Common.ComponentStorage
{
    public interface ITransactionStorage
    {
        List<IStorageTransactionProcessor> GetStorageTransactionProcessors();
    }
}