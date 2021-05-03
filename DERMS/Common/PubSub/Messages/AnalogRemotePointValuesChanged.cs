using Common.GDA;
using NServiceBus;
using System.Collections.Generic;

namespace Common.PubSub.Messages
{
    public class AnalogRemotePointValuesChanged : List<ResourceDescription>, IEvent
    {
        
    }
}
