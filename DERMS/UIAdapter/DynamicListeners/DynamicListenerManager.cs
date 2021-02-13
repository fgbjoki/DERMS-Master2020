using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIAdapter.DynamicListeners
{
    public class DynamicListenerManager : IDisposable
    {
        private EndpointConfiguration endpointConfiguration;

        public DynamicListenerManager()
        {
            endpointConfiguration = new EndpointConfiguration("UIAdapter");

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void AddDynamicListeners()
        {

        }
    }
}
