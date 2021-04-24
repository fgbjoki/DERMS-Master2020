using ClientUI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace ClientUI.ViewModels.CommandingWindow
{
    public abstract class BaseCommandingViewModel : BaseViewModel
    {
        public BaseCommandingViewModel(string title)
        {
            Title = title;

            CloseWindowCommand = new RelayCommand(CloseWindow, null);
        }

        public string Title { get; set; }

        public ICommand CloseWindowCommand { get; set; }

        public void CloseWindow(object param)
        {
            Window window = param as Window;
            window.Close();
        }
    }
}
