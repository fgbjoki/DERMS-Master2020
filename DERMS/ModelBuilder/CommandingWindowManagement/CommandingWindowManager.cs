using ClientUI.CommandingWindowManagement.CommandingWindowHandlers;

namespace ClientUI.CommandingWindowManagement
{
    public class CommandingWindowManager
    {
        private AnalogRemotePointCommandingWindowHandler analogCommandingWindowHandler;
        public CommandingWindowManager()
        {
            InitializeCommandingWindowHandlers();
        }

        private void InitializeCommandingWindowHandlers()
        {
            analogCommandingWindowHandler = new AnalogRemotePointCommandingWindowHandler();
        }
    }
}
