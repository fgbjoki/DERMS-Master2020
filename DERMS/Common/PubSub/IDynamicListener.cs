using Common.PubSub.Subscriptions;

namespace Common.PubSub
{
    public interface IDynamicListener
    {
        void Subscribe(ISubscription subscription);
        Topic Topic { get; }
    }
}
