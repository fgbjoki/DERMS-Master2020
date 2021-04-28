using Common.Communication;
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

        public bool ValidateCommand(long breakerGid, int breakerValue)
        {
            bool commandValid = false;
            try
            {
                commandValid = breakerValidationProxy.Proxy.ValidateCommand(breakerGid, breakerMessageMapping.MapRawDataToBreakerState(breakerValue));
            }
            catch (Exception e)
            {
                Logger.Instance.Log($"[{GetType().Name}] Command invalid. More info:\n{e.Message}\nStack trace:\n{e.StackTrace}");
            }

            return commandValid;
        }

        public bool SendBreakerCommand(long breakerGid, int breakerValue)
        {
            bool commandSent = false;
            try
            {
                commandSent = breakerValidationProxy.Proxy.SendCommand(breakerGid, breakerValue);
            }
            catch (Exception e)
            {
                Logger.Instance.Log($"[{GetType().Name}] Command could not be sent. More info:\n{e.Message}\nStack trace:\n{e.StackTrace}");
            }

            return commandSent;
        }
    }
}
