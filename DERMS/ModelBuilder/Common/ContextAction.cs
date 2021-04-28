using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClientUI.Common
{
    public class ContextAction : BindableBase
    {
        public ContextAction()
        {

        }

        public ContextAction(string header, ICommand command)
        {
            Header = header;
            Command = command;
        }

        public string Header { get; private set; }

        public ICommand Command { get; private set; }
    }
}
