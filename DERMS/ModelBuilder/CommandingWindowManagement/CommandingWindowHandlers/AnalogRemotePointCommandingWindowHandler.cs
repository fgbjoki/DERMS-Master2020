using ClientUI.Events.OpenCommandingWindow;
using System.Windows;
using ClientUI.ViewModels.CommandingWindow;
using ClientUI.Views.CommandingWindow;
using System;
using ClientUI.CustomControls;

namespace ClientUI.CommandingWindowManagement.CommandingWindowHandlers
{
    public class AnalogRemotePointCommandingWindowHandler : CommandingWindowHandler<AnalogRemotePointOpenCommandingWindowEvent, AnalogRemotePointOpenCommandingWindowEventArgs>
    {
        protected override BaseCommandingViewModel CreateViewModel(AnalogRemotePointOpenCommandingWindowEventArgs args)
        {
            return new AnalogRemotePointCommandingViewModel(args);
        }

        protected override CommandWindow CreateWindow(BaseCommandingViewModel viewModel)
        {
            CommandWindow window = new AnalogRemotePointCommandingWindowView();
            window.DataContext = viewModel;

            return window;
        }

        internal override string GetWindowTitle(AnalogRemotePointOpenCommandingWindowEventArgs args)
        {
            return $"{args.Name} Commanding Window";
        }
    }
}
