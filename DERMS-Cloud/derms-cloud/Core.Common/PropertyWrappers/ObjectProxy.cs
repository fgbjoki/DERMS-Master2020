namespace Core.Common.ListenerDepedencyInjection
{
    public class ObjectProxy<T>
        where T : class
    {
        public T Instance { get; set; }
    }
}
