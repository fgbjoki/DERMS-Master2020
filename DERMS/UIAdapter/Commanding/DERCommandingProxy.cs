using Common.ServiceInterfaces.UIAdapter;
using Common.DataTransferObjects;
using Common.ServiceInterfaces.CalculationEngine;
using Common.Communication;
using Common.DataTransferObjects.CalculationEngine;

namespace UIAdapter.Commanding
{
    public class DERCommandingProxy : IDERCommanding
    {
        private WCFClient<IDERCommandingProcessor> derCommandingProxy;

        public DERCommandingProxy()
        {
            derCommandingProxy = new WCFClient<IDERCommandingProcessor>("ceDERCommanding");
        }

        public CommandFeedbackMessageDTO SendCommand(long derGid, float commandingValue)
        {
            CommandFeedbackMessageDTO feedbackMessage;

            try
            {
                var ceFeedback = derCommandingProxy.Proxy.Command(derGid, commandingValue);
                feedbackMessage = CreateClientDTO(ceFeedback);
            }
            catch
            {
                feedbackMessage = new CommandFeedbackMessageDTO()
                {
                    CommandExecuted = false,
                    Message = "Service is not available"
                };
            }

            return feedbackMessage;
        }

        public CommandFeedbackMessageDTO ValidateCommand(long derGid, float commandingValue)
        {
            CommandFeedbackMessageDTO feedbackMessage;

            try
            {
                var ceFeedback = derCommandingProxy.Proxy.ValidateCommand(derGid, commandingValue);
                feedbackMessage = CreateClientDTO(ceFeedback);
            }
            catch
            {
                feedbackMessage = new CommandFeedbackMessageDTO()
                {
                    CommandExecuted = false,
                    Message = "Service is not available"
                };
            }

            return feedbackMessage;
        }

        private CommandFeedbackMessageDTO CreateClientDTO(CommandFeedback ceCommandFeedback)
        {
            return new CommandFeedbackMessageDTO()
            {
                CommandExecuted = ceCommandFeedback.Successful,
                Message = ceCommandFeedback.Message
            };
        }
    }
}
