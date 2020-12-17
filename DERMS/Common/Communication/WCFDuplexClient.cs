using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Common.Communication
{
    public class WCFDuplexClient<T> : DuplexChannelFactory<T>
    {
        private InstanceContext callBackInstance;
        public WCFDuplexClient(object callBackObject, Binding binding, EndpointAddress endpointAddress) : base(callBackObject, binding, endpointAddress)
        {
            callBackInstance = new InstanceContext(callBackObject);
        }

        public T Proxy
        {
            get
            {
                
                return CreateChannel(callBackInstance);
            }
        }
    }
}
