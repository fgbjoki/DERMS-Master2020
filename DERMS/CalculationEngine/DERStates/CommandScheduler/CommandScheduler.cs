using CalculationEngine.Commanding.Commands;
using Common.Communication;
using Common.Logger;
using Common.ServiceInterfaces.NetworkDynamicsService.Commands;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CalculationEngine.DERStates.CommandScheduler
{
    public class CommandScheduler : ICommandTimerElapsedHandler, ICommandScheduler
    {
        private static readonly int maxWaitInterval = 2000;

        private ReaderWriterLock locker;

        private Dictionary<long, TimedCommand> commandsScheduled;

        private WCFClient<INDSCommanding> ndsCommanding;

        public CommandScheduler()
        {
            locker = new ReaderWriterLock();

            commandsScheduled = new Dictionary<long, TimedCommand>();

            ndsCommanding = new WCFClient<INDSCommanding>("ndsCommanding");
        }

        public void ScheduleCommand(TimedCommand timedCommand)
        {
            RemoveScheduledCommand(timedCommand.Command.GlobalId);

            locker.AcquireWriterLock(maxWaitInterval);

            timedCommand.SetHandler(this);

            commandsScheduled.Add(timedCommand.Command.GlobalId, timedCommand);

            timedCommand.StartTimer();

            locker.ReleaseWriterLock();

            Logger.Instance.Log($"[{GetType().Name}] Successfuly scheduled command {timedCommand.Command.GetType().Name} with gid: 0x{timedCommand.Command.GlobalId:X16}.");
        }

        public void RemoveScheduledCommand(long commandId)
        {
            locker.AcquireWriterLock(maxWaitInterval);

            TimedCommand command;
            if (!commandsScheduled.TryGetValue(commandId, out command))
            {
                locker.ReleaseWriterLock();
                return;
            }

            command.Dispose();
            commandsScheduled.Remove(commandId);

            locker.ReleaseWriterLock();

            Logger.Instance.Log($"[{GetType().Name}] Successfuly removed command {command.Command.GetType().Name} with gid: 0x{command.Command.GlobalId:X16}.");
        }

        public void CommandTimerElapsed(Command command)
        {
            RemoveScheduledCommand(command.GlobalId);

            try
            {
                if (ndsCommanding.Proxy.SendCommand(command.CreateNDSCommand()))
                {
                    Logger.Instance.Log($"[{GetType().Name}] Successfuly sent scheduled command {command.GetType().Name} with gid: 0x{command.GlobalId:X16}.");
                }
                else
                {
                    Logger.Instance.Log($"[{GetType().Name}] Scheduled command {command.GetType().Name} with gid: 0x{command.GlobalId:X16} couldnt be send.");
                }
            }
            catch (Exception e)
            {
                Logger.Instance.Log($"[{GetType().Name}] Scheduled command {command.GetType().Name} with gid: 0x{command.GlobalId:X16} couldnt be send.");
                Logger.Instance.Log($"[{GetType().Name}] Exception occured while sending scheduled command: {e.GetType().Name}\nMessage:{e.Message}");
            }
        }

        public void PauseScheduledCommand(long commandId)
        {
            locker.AcquireReaderLock(maxWaitInterval);

            TimedCommand timedCommand = GetTimedCommand(commandId);
            if (timedCommand == null)
            {
                locker.ReleaseReaderLock();
                return;
            }

            locker.AcquireWriterLock(maxWaitInterval);

            timedCommand.StopTimer();

            locker.ReleaseWriterLock();
        }

        public void ResumeScheduledCommand(long commandId)
        {
            locker.AcquireReaderLock(maxWaitInterval);

            TimedCommand timedCommand = GetTimedCommand(commandId);
            if (timedCommand == null)
            {
                locker.ReleaseReaderLock();
                return;
            }

            locker.AcquireWriterLock(maxWaitInterval);

            timedCommand.StartTimer();

            locker.ReleaseWriterLock();
        }

        private TimedCommand GetTimedCommand(long commandId)
        {
            TimedCommand timedCommand;

            commandsScheduled.TryGetValue(commandId, out timedCommand);

            return timedCommand;
        }
    }
}
