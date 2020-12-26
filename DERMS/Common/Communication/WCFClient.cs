using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Common.Communication
{
    public class WCFClient<T> : ChannelFactory<T>
    {
        public WCFClient() : base()
        {

        }

        public WCFClient(string endpointConfigurationName) : base(endpointConfigurationName)
        {

        }

        public WCFClient(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress)
        {

        }

        public T Proxy
        {
            get { return CreateChannel(); }
        }
    }
}
