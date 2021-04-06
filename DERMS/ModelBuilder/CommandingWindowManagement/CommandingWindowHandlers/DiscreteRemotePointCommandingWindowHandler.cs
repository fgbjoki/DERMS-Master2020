using ClientUI.CustomControls;
using ClientUI.Events.OpenCommandingWindow;
using ClientUI.ViewModels.CommandingWindow;
using ClientUI.Views.CommandingWindow;
using System;
using System.Windows;

namespace ClientUI.CommandingWindowManagement.CommandingWindowHandlers
{
    public class DiscreteRemotePointCommandingWindowHandler : CommandingWindowHandler<DiscreteRemotePointOpenCommandingWindowEvent, DiscreteRemotePointOpenCommandingWindowEventArgs>
    {
        protected override CommandingViewModel CreateViewModel(DiscreteRemotePointOpenCommandingWindowEventArgs args)
        {
            return new DiscreteRemotePointCommandingViewModel(args);
        }

        protected override CommandWindow CreateWindow(CommandingViewModel viewModel)
        {
            CommandWindow window = new DiscreteRemotePointCommandingWindowView();
            window.DataContext = viewModel;

            return window;
        }

        internal override string GetWindowTitle(DiscreteRemotePointOpenCommandingWindowEventArgs args)
        {
            return $"{args.Name} Commanding Window";
        }
    }
}
