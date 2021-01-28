using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.ViewModels.CommandingWindow
{
    public class CommandingViewModel : BaseViewModel
    {
        public CommandingViewModel(string title)
        {
            Title = title;
        }

        public string Title { get; set; }
    }
}
