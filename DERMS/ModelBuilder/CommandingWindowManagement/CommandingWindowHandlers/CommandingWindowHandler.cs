using ClientUI.CustomControls;
using ClientUI.Events.OpenCommandingWindow;
using ClientUI.SummaryCreator;
using ClientUI.ViewModels.CommandingWindow;
using System.Collections.Generic;
using System.Windows;

namespace ClientUI.CommandingWindowManagement.CommandingWindowHandlers
{
    public abstract class CommandingWindowHandler<TEventType, TEventArgType>
        where TEventType : OpenCommandingWindowEvent<TEventArgType>, new()
        where TEventArgType : OpenCommandingWindowEventArgs
    {
        private Dictionary<long, CommandWindow> openedWindows;

        protected CommandingWindowHandler()
        {
            openedWindows = new Dictionary<long, CommandWindow>();

            SummaryManager.Instance.EventAggregator.GetEvent<TEventType>().Subscribe(OpenNewWindow);
        }

        protected bool GetWindow(long gid, out CommandWindow window)
        {
            return openedWindows.TryGetValue(gid, out window);
        }

        protected virtual void OpenNewWindow(TEventArgType args)
        {
            CommandWindow window;
            if (GetWindow(args.GlobalId, out window))
            {
                window.WindowState = WindowState.Normal;
                window.Focus();
            }
            else
            {               
                window = ConfigureWindow(args);

                window.Show();

                openedWindows.Add(args.GlobalId, window);
            }
        }

        private CommandWindow ConfigureWindow(TEventArgType args)
        {
            BaseCommandingViewModel windowViewModel = CreateViewModel(args);

            CommandWindow window = CreateWindow(windowViewModel);
            window.Title = GetWindowTitle(args);
            window.GlobalId = args.GlobalId;
            window.Closing += Window_Closing;

            return window;
        }

        internal abstract string GetWindowTitle(TEventArgType args);

        protected abstract BaseCommandingViewModel CreateViewModel(TEventArgType args);

        protected abstract CommandWindow CreateWindow(BaseCommandingViewModel viewModel);

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CommandWindow commandWindow = sender as CommandWindow;
            openedWindows.Remove(commandWindow.GlobalId);
        }
    }
}
