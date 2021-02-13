using NServiceBus;
using System.Collections.Generic;

namespace UIAdapter.TransactionProcessing.Storages
{
    public interface INServiceBusStorage
    {
        List<object> GetHandlers();
    }
}
