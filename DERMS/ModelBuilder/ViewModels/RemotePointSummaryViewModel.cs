using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.ViewModels
{
    public class RemotePointSummaryViewModel : ContentViewModel
    {
        public RemotePointSummaryViewModel() : base("Remote Point Summary", SummaryCreator.ContentType.RemotePointSummary)
        {

        }
    }
}
