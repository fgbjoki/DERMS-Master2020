using ClientUI.CustomControls;
using ClientUI.Events.OpenCommandingWindow;
using ClientUI.ViewModels.CommandingWindow;
using ClientUI.ViewModels.CommandingWindow.DERGroup;
using ClientUI.Views.CommandingWindow.DER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.CommandingWindowManagement.CommandingWindowHandlers
{
    public class DERGroupCommandingWIndowHandler : CommandingWindowHandler<DERGroupOpenCommandingWindowEvent, DERGroupOpenCommandingWindowEventArgs>
    {
        protected override BaseCommandingViewModel CreateViewModel(DERGroupOpenCommandingWindowEventArgs args)
        {
            return new DERGroupCommandingWindowViewModel(args);
        }

        protected override CommandWindow CreateWindow(BaseCommandingViewModel viewModel)
        {
            CommandWindow window = new DERGroupCommandingWindowView();
            window.DataContext = viewModel;

            return window;
        }

        internal override string GetWindowTitle(DERGroupOpenCommandingWindowEventArgs args)
        {
            return $"DER Group Commanding";
        }
    }
}
