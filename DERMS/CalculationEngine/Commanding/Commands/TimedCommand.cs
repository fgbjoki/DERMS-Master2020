using System;
using System.Timers;

namespace CalculationEngine.Commanding.Commands
{
    public class TimedCommand : IDisposable
    {
        private ICommandTimerElapsedHandler handler;

        private Timer timer;

        public TimedCommand(double timeInterval, Command command)
        {
            Command = command;

            timer = new Timer();
            timer.AutoReset = false;
            timer.Interval = timeInterval;
            timer.Elapsed += Timer_Elapsed;
        }

        public Command Command { get; private set; }

        public void SetHandler(ICommandTimerElapsedHandler handler)
        {
            this.handler = handler;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            handler.CommandTimerElapsed(Command);
        }

        public void StartTimer()
        {
            timer.Start();
        }

        public void StopTimer()
        {
            timer.Stop();
        }

        public void Dispose()
        {
            timer.Elapsed -= Timer_Elapsed;

            timer.Stop();
            timer.Dispose();
        }
    }
}
