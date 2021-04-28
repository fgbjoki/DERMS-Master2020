using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClientUI.Commanding
{
    public class CommandFeedback : BindableBase
    {
        private bool commandExecuted;
        private string message;
        private Visibility visibility;

        public CommandFeedback(string message, bool commandExecuted)
        {
            visibility = Visibility.Hidden;
        }

        public bool CommandExecuted
        {
            get { return commandExecuted; }
            set
            {
                SetProperty(ref commandExecuted, value);
            }
        }

        public string Message
        {
            get { return message; }
            set
            {
                if (message != value)
                {
                    SetProperty(ref message, value);
                }
            }
        }

        public Visibility Visibility
        {
            get { return visibility; }
            set
            {
                if (visibility != value)
                {
                    SetProperty(ref visibility, value);
                }
            }
        }
    }
}
