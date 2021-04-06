using Common.PubSub.Subscriptions;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.PubSub
{
    public abstract class BaseMessageListener<T> : IHandleMessages<T>, IDynamicListener
        where T : IEvent
    {
        private ICollection<IDynamicHandler> subscribers;

        protected BaseMessageListener(Topic topic)
        {
            subscribers = new List<IDynamicHandler>();

            Topic = topic;
        }

        public Topic Topic { get; private set; }

        public virtual Task Handle(T message, IMessageHandlerContext context)
        {
            Logger.Logger.Instance.Log($"[{GetType()}] New publication.");
            if (message == null)
            {
                Logger.Logger.Instance.Log($"[{this.GetType()}] There was no data published. Skipping further processing!");
                return Task.CompletedTask;
            }

            foreach (var subscriber in subscribers)
            {
                try
                {
                    subscriber.ProcessChanges(message);
                }
                catch (Exception e)
                {
                    Logger.Logger.Instance.Log($"[{this.GetType()}] Exception occured on {subscriber.GetType()} details: {e.Message}, stack trace: {e.StackTrace}");
                }
            }

            return Task.CompletedTask;
        }

        public void Subscribe(ISubscription subscription)
        {
            if (Topic != subscription.Topic)
            {
                Logger.Logger.Instance.Log($"[{this.GetType()}] Cannot add subscriber of type {subscription.Subscriber.GetType()} to listener {this.GetType()} due to topic missmatch (targeted topic {subscription.Topic.ToString()}).");
                return;
            }

            subscribers.Add(subscription.Subscriber);
        }
    }
}
