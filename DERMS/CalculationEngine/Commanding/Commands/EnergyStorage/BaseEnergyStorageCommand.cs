using Common.ServiceInterfaces.NetworkDynamicsService.Commands;

namespace CalculationEngine.Commanding.Commands
{
    public abstract class BaseEnergyStorageCommand : Command
    {
        public virtual float ActivePower { get; set; }

        public double SecondsOfUse { get; set; }

        public override BaseCommand CreateNDSCommand()
        {
            ChangeAnalogRemotePointValue ndsAnalogCommand = new ChangeAnalogRemotePointValue()
            {
                GlobalId = GlobalId,
                Value = ActivePower
            };

            return ndsAnalogCommand;
        }
    }
}
