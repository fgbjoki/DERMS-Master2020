using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.ViewModels
{
    public abstract class ContentViewModel : BaseViewModel
    {       
        public ContentViewModel(string pageName)
        {
            PageName = pageName;
        }

        public string PageName { get; set; }
    }
}
