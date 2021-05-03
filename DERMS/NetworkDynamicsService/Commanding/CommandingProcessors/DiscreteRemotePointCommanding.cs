using NetworkDynamicsService.Model.RemotePoints;
using System;
using Common.ComponentStorage;
using Common.ServiceInterfaces.NetworkDynamicsService.Commands;
using Common.Logger;
using Common.SCADA.FieldProcessor;

namespace NetworkDynamicsService.Commanding.CommandigProcessors
{
    public class DiscreteRemotePointCommanding : BaseCommandingProcessor<DiscreteRemotePoint>
    {
        public DiscreteRemotePointCommanding(IStorage<DiscreteRemotePoint> storage) : base(storage)
        {
        }

        public override bool ProcessCommand(BaseCommand command)
        {
            ChangeDiscreteRemotePointValue discreteCommand = command as ChangeDiscreteRemotePointValue;
            if (discreteCommand == null)
            {
                Logger.Instance.Log($"[{GetType().Name}] Couldn't process discrete command with commanded gid: 0x{command.GlobalId:X16}!");
                return false;
            }

            var remotePoint = GetRemotePoint(command.GlobalId);

            if (remotePoint.CurrentValue == discreteCommand.Value)
            {
                return true;
            }

            try
            {
                return fieldProcessorClient.Proxy.SendCommand(new ChangeRemotePointValueCommand(remotePoint.GlobalId, discreteCommand.Value));
            }
            catch (Exception e)
            {
                Logger.Instance.Log($"[{GetType().Name}] Couldn't process discrete command with commanded gid: 0x{command.GlobalId:X16}! Exception info:\n{e.Message}. Stacktrace:\n{e.StackTrace}.");
                return false;
            }
        }
    }
}
