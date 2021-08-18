using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClientUI.ViewModels.DEROptimalCommanding.OptimalCommanding.CommandParameterViewModels.Commands
{
    public class CalculateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            double setPointValue;

            if (!double.TryParse(parameter as string, out setPointValue))
            {
                return false;
            }

            return setPointValue != 0;
        }

        public void Execute(object parameter)
        {
            // parameter = setpoint
            // TODO CALL WCF SERVICE FOR UIAdapter
        }
    }
}
