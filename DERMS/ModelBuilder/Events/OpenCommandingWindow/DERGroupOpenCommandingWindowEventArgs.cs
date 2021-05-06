using ClientUI.ViewModels.CommandingWindow.DERGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.Events.OpenCommandingWindow
{
    public class DERGroupOpenCommandingWindowEventArgs : OpenCommandingWindowEventArgs
    {
        public DERView DERView { get; set; }
    }
}
