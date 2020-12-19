using FieldSimulator.Modbus;
using System;
using System.Windows.Input;

namespace FieldSimulator.Commands
{
    class SimulatorStartCommand : ICommand
    {
        private ModbusSlave slave;

        public SimulatorStartCommand(ModbusSlave slave)
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
            return !slave.ServerStarted;
        }

        public virtual void Execute(object parameter)
        {
            slave.StartServer();
        }
    }
}
