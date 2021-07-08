using System.Threading;
using System.Threading.Tasks;

namespace PollingService.Service
{
    public interface IPollingInvoker
    {
        Task StartAquisition(CancellationToken cancellationToken);
    }
}