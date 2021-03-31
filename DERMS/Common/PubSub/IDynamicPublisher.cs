using NServiceBus;
using System.Threading.Tasks;

namespace Common.PubSub
{
    public interface IDynamicPublisher
    {
        Task Publish(IEvent message);
    }
}