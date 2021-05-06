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
        private int height;
        private int width;

        public BaseCommandingViewModel(string title)
        {
            Title = title;

            CloseWindowCommand = new RelayCommand(CloseWindow, null);

            Height = 600;
            Width = 500;
        }

        public string Title { get; set; }

        public ICommand CloseWindowCommand { get; set; }

        public int Height
        {
            get { return height; }
            set
            {
                if (height != value)
                {
                    SetProperty(ref height, value);
                }
            }
        }

        public int Width
        {
            get { return width; }
            set
            {
                if (width != value)
                {
                    SetProperty(ref width, value);
                }
            }
        }

        protected abstract void StopProcessing();

        public void CloseWindow(object param)
        {
            StopProcessing();

            Window window = param as Window;
            window.Close();
        }
    }
}
