using System.Collections.Generic;

namespace Common.PubSub
{
    public interface INServiceBusHandlerCreator
    {
        List<object> GetHandlers();
    }
}
