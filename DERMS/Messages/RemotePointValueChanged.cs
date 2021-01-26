using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages
{
    public class RemotePointValueChanged : IEvent
    {
        public long GID { get; set; }
        public ushort Value { get; set; }
    }
}
