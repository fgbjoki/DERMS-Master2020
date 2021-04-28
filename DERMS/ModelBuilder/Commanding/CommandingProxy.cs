using Common.Communication;
using Common.ServiceInterfaces.UIAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.Commanding
{
    public class CommandingProxy : IBreakerCommanding
    {
        private static CommandingProxy instance;
        private static WCFClient<IBreakerCommanding> breakerCommandingProxy;

        static CommandingProxy()
        {
            instance = new CommandingProxy();
            breakerCommandingProxy = new WCFClient<IBreakerCommanding>("uiAdapterBreakerCommanding");
        }

        public static CommandingProxy Instance { get { return instance; } }

        public bool SendBreakerCommand(long breakerGid, int breakerValue)
        {
            try
            {
                return breakerCommandingProxy.Proxy.SendBreakerCommand(breakerGid, breakerValue);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool ValidateCommand(long breakerGid, int breakerValue)
        {
            try
            {
                return breakerCommandingProxy.Proxy.ValidateCommand(breakerGid, breakerValue);
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
