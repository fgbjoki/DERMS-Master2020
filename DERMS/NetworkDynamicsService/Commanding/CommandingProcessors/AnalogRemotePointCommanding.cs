using NetworkDynamicsService.Model.RemotePoints;
using System;
using Common.ComponentStorage;
using Common.ServiceInterfaces.NetworkDynamicsService.Commands;
using Common.Logger;
using Common.SCADA.FieldProcessor;

namespace NetworkDynamicsService.Commanding.CommandigProcessors
{
    public class AnalogRemotePointCommanding : BaseCommandingProcessor<AnalogRemotePoint>
    {
        public AnalogRemotePointCommanding(IStorage<AnalogRemotePoint> storage) : base(storage)
        {
        }

        public override bool ProcessCommand(BaseCommand command)
        {
            ChangeAnalogRemotePointValue analogCommand = command as ChangeAnalogRemotePointValue;
            if (analogCommand == null)
            {
                Logger.Instance.Log($"[{GetType().Name}] Couldn't process analog command with commanded gid: 0x{command.GlobalId:X16}!");
                return false;
            }

            var remotePoint = GetRemotePoint(command.GlobalId);

            if (remotePoint.CurrentValue == analogCommand.Value)
            {
                return true;
            }

            try
            {
                return fieldProcessorClient.Proxy.SendCommand(new ChangeRemotePointValueCommand(remotePoint.GlobalId, ConvertFloatValue(analogCommand.Value)));
            }
            catch (Exception e)
            {
                Logger.Instance.Log($"[{GetType().Name}] Couldn't process analog command with commanded gid: 0x{command.GlobalId:X16}! Exception info:\n{e.Message}. Stacktrace:\n{e.StackTrace}.");
                return false;
            }
        }

        private int ConvertFloatValue(float value)
        {
            int returnValue;
            unsafe
            {
                float* intPointer = (float*)&returnValue;
                *intPointer = value;
            }

            return returnValue;
        }
    }
}
