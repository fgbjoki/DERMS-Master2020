namespace Common.PubSub
{
    public abstract class BaseDynamicHandler<DynamicMessageType> : IDynamicHandler
        where DynamicMessageType : class
    {
        public void ProcessChanges(object changes)
        {
            ProcessChanges(changes as DynamicMessageType);
        }

        protected abstract void ProcessChanges(DynamicMessageType message);
    }
}
