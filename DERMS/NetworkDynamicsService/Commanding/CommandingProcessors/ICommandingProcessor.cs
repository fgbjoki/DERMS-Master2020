using Common.ServiceInterfaces.NetworkDynamicsService.Commands;

namespace NetworkDynamicsService.Commanding.CommandigProcessors
{
    public interface ICommandingProcessor
    {
        bool ProcessCommand(BaseCommand command);
    }
}