using NServiceBus;
using System.Collections.Generic;

namespace Common.PubSub.Messages
{
    public class BaseMessageEntitiesChanged<T> : IEvent
    {
        public List<T> Changes { get; set; } = new List<T>();
    }
}
