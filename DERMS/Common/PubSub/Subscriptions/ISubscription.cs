namespace Common.PubSub.Subscriptions
{
    public interface ISubscription
    {
        Topic Topic { get; }
        IDynamicHandler Subscriber { get; }
    }
}
