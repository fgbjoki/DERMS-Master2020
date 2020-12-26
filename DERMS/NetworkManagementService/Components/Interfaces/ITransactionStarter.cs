using System.Collections.Generic;

namespace NetworkManagementService.Components
{
    public interface ITransactionStarter
    {
        bool StartTransaction(IEnumerable<long> insertedGids);
    }
}
