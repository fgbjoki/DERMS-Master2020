using Common.PubSub.Messages.DERState;
using NServiceBus;
using System.Collections.Generic;

namespace Common.PubSub.Messages
{
    public class DERStateChanged : IEvent
    {
        public DERStateChanged()
        {
            DERStates = new List<DERStateWrapper>();
        }

        public List<DERStateWrapper> DERStates { get; set; }
    }
}
