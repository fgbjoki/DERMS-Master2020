using ClientUI.CommandingWindowManagement.CommandingWindowHandlers;

namespace ClientUI.CommandingWindowManagement
{
    public class CommandingWindowManager
    {
        private AnalogRemotePointCommandingWindowHandler analogCommandingWindowHandler;
        private DiscreteRemotePointCommandingWindowHandler discreteCommandingWindowHandler;


        public CommandingWindowManager()
        {
            InitializeCommandingWindowHandlers();
        }

        private void InitializeCommandingWindowHandlers()
        {
            analogCommandingWindowHandler = new AnalogRemotePointCommandingWindowHandler();
            discreteCommandingWindowHandler = new DiscreteRemotePointCommandingWindowHandler();
        }
    }
}
