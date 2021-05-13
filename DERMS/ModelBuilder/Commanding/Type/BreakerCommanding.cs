using Common.Communication;
using Common.DataTransferObjects;
using Common.ServiceInterfaces.UIAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.Commanding.Type
{
    public class BreakerCommanding : IBreakerCommanding
    {
        private WCFClient<IBreakerCommanding> breakerCommandingProxy;

        public BreakerCommanding()
        {
            breakerCommandingProxy = new WCFClient<IBreakerCommanding>("uiAdapterBreakerCommanding");
        }

        public CommandFeedbackMessageDTO SendBreakerCommand(long breakerGid, int breakerValue)
        {
            try
            {
                return breakerCommandingProxy.Proxy.SendBreakerCommand(breakerGid, breakerValue);
            }
            catch (Exception e)
            {
                return new CommandFeedbackMessageDTO()
                {
                    CommandExecuted = false,
                    Message = "Command couldn't be executed. Check logs"
                };
            };
        }

        public CommandFeedbackMessageDTO ValidateCommand(long breakerGid, int breakerValue)
        {
            try
            {
                return breakerCommandingProxy.Proxy.ValidateCommand(breakerGid, breakerValue);
            }
            catch (Exception e)
            {
                return new CommandFeedbackMessageDTO()
                {
                    CommandExecuted = false,
                    Message = "Command couldn't be executed. Check logs"
                };
            }
        }
    }
}
