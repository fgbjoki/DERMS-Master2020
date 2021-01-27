using ClientUI.SummaryCreator;
using Microsoft.Practices.ServiceLocation;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private ContentViewModel contentViewModel;

        public MainWindowViewModel()
        {
            SummaryManager.Instance.EventAggregator.GetEvent<ChangeSummaryEvent>().Subscribe(ChangeViewModel);
        }

        private void ChangeViewModel(ChangeSummaryEventArgs eventArgs)
        {
            ContentViewModel = SummaryManager.Instance.ViewModelContainer.GetContent(eventArgs.SummaryType);
        }

        public ContentViewModel ContentViewModel
        {
            get { return contentViewModel; }
            set { SetProperty(ref contentViewModel, value); }
        }
    }
}
