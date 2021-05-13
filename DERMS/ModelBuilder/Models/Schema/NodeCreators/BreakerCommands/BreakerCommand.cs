using ClientUI.Commanding;
using ClientUI.Common.MessageBox;
using ClientUI.Models.Schema.Nodes;
using Common.DataTransferObjects;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ClientUI.Models.Schema.NodeCreators.BreakerCommands
{
    public class BreakerCommand : ICommand
    {
        private SchemaBreakerNode breakerNode;
        private bool breakerStateCondition;

        public BreakerCommand(SchemaBreakerNode breakerNode, bool breakerStateCondition)
        {
            this.breakerNode = breakerNode;
            this.breakerStateCondition = breakerStateCondition;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return breakerNode.Closed == breakerStateCondition;
        }

        public void Execute(object parameter)
        {
            // if breaker state command condition is false (OPEN) then command should close the breaker (0 value), otherwise open the breaker (1 value)
            int breakerCommandingState = breakerStateCondition ? 1 : 0;
            CommandFeedbackMessageDTO feedback = CommandingProxy.Instance.BreakerCommanding.SendBreakerCommand(breakerNode.GlobalId, breakerCommandingState);
            
            if (feedback.CommandExecuted)
            {
                MessageBoxCreator.Show("Command successfuly sent!", "Command information", PackIconKind.Information);
            }
            else
            {
                MessageBoxCreator.Show($"{feedback.Message}", "Command information", PackIconKind.Error);
            }
        }
    }
}
