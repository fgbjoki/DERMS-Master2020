using System.Collections.Generic;

namespace UIAdapter.TransactionProcessing.Storages
{
    public interface INServiceBusHandlerCreator
    {
        List<object> GetHandlers();
    }
}
