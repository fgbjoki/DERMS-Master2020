using ClientUI.Events.ChangeSummary;
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
            if (ContentViewModel != null)
            {
                ContentViewModel.StopProcessing();
            }

            ContentViewModel = SummaryManager.Instance.ViewModelContainer.GetContent(eventArgs.SummaryType);

            ContentViewModel.StartProcessing();
        }

        public ContentViewModel ContentViewModel
        {
            get { return contentViewModel; }
            set { SetProperty(ref contentViewModel, value); }
        }
    }
}
