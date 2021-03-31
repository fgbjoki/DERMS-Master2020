namespace Common.PubSub
{
    public interface IDynamicHandler
    {
        void ProcessChanges(object changes);
    }
}
