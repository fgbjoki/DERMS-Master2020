namespace Common.PubSub.Subscriptions
{
    public class Subscription : ISubscription
    {
        public Subscription(Topic topic, IDynamicHandler subscriber)
        {
            Subscriber = subscriber;
            Topic = topic;
        }

        public IDynamicHandler Subscriber { get; private set; }

        public Topic Topic { get; private set; }
    }
}
