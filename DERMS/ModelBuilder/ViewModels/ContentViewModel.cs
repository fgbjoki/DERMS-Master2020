using ClientUI.SummaryCreator;
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
        public ContentViewModel(string pageName, ContentType contentType)
        {
            PageName = pageName;
            ContentType = contentType;
        }

        public ContentType ContentType { get; private set; }

        public string PageName { get; private set; }
    }
}
