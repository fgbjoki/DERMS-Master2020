using Common.ServiceInterfaces.UIAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DataTransferObjects;
using Common.Communication;

namespace ClientUI.Commanding.Type
{
    public class DERCommanding : IDERCommanding
    {
        private WCFClient<IDERCommanding> derCommandingClient;

        public DERCommanding()
        {
            derCommandingClient = new WCFClient<IDERCommanding>("uiAdapterDERCommanding");
        }

        public CommandFeedbackMessageDTO SendCommand(long derGid, float commandingValue)
        {
            try
            {
                return derCommandingClient.Proxy.SendCommand(derGid, commandingValue);
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

        public CommandFeedbackMessageDTO ValidateCommand(long derGid, float commandingValue)
        {
            try
            {
                return derCommandingClient.Proxy.ValidateCommand(derGid, commandingValue);
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
    }
}
