using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Common.Communication
{
    public class WCFClient<T> : ChannelFactory<T>
    {
        public WCFClient(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress)
        {

        }

        public T Proxy
        {
            get { return CreateChannel(); }
        }
    }
}
