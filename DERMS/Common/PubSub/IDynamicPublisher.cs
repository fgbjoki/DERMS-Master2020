using NServiceBus;

namespace Common.PubSub
{
    public interface IDynamicPublisher
    {
        void Publish(IEvent message);
    }
}