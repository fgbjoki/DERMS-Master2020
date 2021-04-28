using ClientUI.Models.Schema.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            // TODO
        }
    }
}
