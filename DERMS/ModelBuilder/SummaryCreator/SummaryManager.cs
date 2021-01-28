using ClientUI.CommandingWindowManagement;
using Prism.Events;

namespace ClientUI.SummaryCreator
{
    public class SummaryManager
    {
        private static SummaryManager instance;
        private static IEventAggregator eventAggreagor;
        private static SummaryViewModelContainer viewModelContainer;
        private static CommandingWindowManager commandingWindowManager;

        static SummaryManager()
        {
            eventAggreagor = new EventAggregator();
            instance = new SummaryManager();
            viewModelContainer = new SummaryViewModelContainer();
            commandingWindowManager = new CommandingWindowManager();
        }

        public static SummaryManager Instance { get { return instance; } }

        public IEventAggregator EventAggregator { get { return eventAggreagor; } }

        public SummaryViewModelContainer ViewModelContainer { get { return viewModelContainer; } }
    }
}
