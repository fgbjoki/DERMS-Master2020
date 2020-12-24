using FieldSimulator.Modbus;
using System;
using System.Windows.Input;

namespace FieldSimulator.Commands
{
    class SimulatorStopCommand : ICommand
    {
        private ModbusSlave slave;

        public SimulatorStopCommand(ModbusSlave slave)
        {
            this.slave = slave;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public virtual bool CanExecute(object parameter)
        {
            return slave.ServerStarted;
        }

        public virtual void Execute(object parameter)
        {
            slave.StopServer();
        }
    }
}
