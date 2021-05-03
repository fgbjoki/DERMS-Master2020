using Common.SCADA;
using NServiceBus;
using System.Collections.Generic;

namespace NetworkDynamicsService.RemotePointProcessors
{
    interface IValueChangedProcessor
    {
        IEvent ProcessChangedValue(IEnumerable<RemotePointFieldValue> fieldValues);
    }
}
