using Common.Communication;
using Common.DataTransferObjects;
using Common.Helpers.Breakers;
using Common.Logger;
using Common.ServiceInterfaces.UIAdapter;
using System;

namespace UIAdapter.Commanding
{
    public class BreakerCommandingProxy : IBreakerCommanding
    {
        private WCFClient<Common.ServiceInterfaces.CalculationEngine.IBreakerCommanding> breakerValidationProxy;
        private BreakerMessageMapping breakerMessageMapping;

        public BreakerCommandingProxy(BreakerMessageMapping breakerMessageMapping)
        {
            this.breakerMessageMapping = breakerMessageMapping;

            breakerValidationProxy = new WCFClient<Common.ServiceInterfaces.CalculationEngine.IBreakerCommanding>("ceBreakerValidation");
        }

        public CommandFeedbackMessageDTO ValidateCommand(long breakerGid, int breakerValue)
        {
            CommandFeedbackMessageDTO feedbackMessage = new CommandFeedbackMessageDTO();
            try
            {
                if (!breakerValidationProxy.Proxy.ValidateCommand(breakerGid, breakerMessageMapping.MapRawDataToBreakerState(breakerValue)))
                {
                    feedbackMessage.Message = "Command will cause loop in distributed network!";
                    feedbackMessage.CommandExecuted = false;
                }
                else
                {
                    feedbackMessage.Message = "Command valid!";
                    feedbackMessage.CommandExecuted = true;
                }
            }
            catch (Exception e)
            {
                Logger.Instance.Log($"[{GetType().Name}] Command invalid. More info:\n{e.Message}\nStack trace:\n{e.StackTrace}");
            }

            return feedbackMessage;
        }

        public CommandFeedbackMessageDTO SendBreakerCommand(long breakerGid, int breakerValue)
        {
            CommandFeedbackMessageDTO feedBack = null;
            try
            {
                feedBack = ValidateCommand(breakerGid, breakerValue);
                if (feedBack.CommandExecuted == false)
                {
                    return feedBack;
                }

                if (breakerValidationProxy.Proxy.SendCommand(breakerGid, breakerValue))
                {
                    feedBack.CommandExecuted = true;
                    feedBack.Message = "Command successfully sent!";
                }
                else
                {
                    feedBack.CommandExecuted = false;
                    feedBack.Message = "Command couldn't be executed!";
                }
            }
            catch (Exception e)
            {
                feedBack.CommandExecuted = false;
                feedBack.Message = "Service couldn't execute the command. Check logs.";
                Logger.Instance.Log($"[{GetType().Name}] Command could not be sent. More info:\n{e.Message}\nStack trace:\n{e.StackTrace}");
            }

            return feedBack;
        }
    }
}
