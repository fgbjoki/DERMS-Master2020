using ClientUI.Commanding.Type;
using Common.Communication;
using Common.DataTransferObjects;
using Common.ServiceInterfaces.UIAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.Commanding
{
    public class CommandingProxy
    {
        private static CommandingProxy instance;       

        private CommandingProxy()
        {
            BreakerCommanding = new BreakerCommanding();
            DERCommanding = new DERCommanding();
        }

        static CommandingProxy()
        {
            instance = new CommandingProxy();           
        }

        public static CommandingProxy Instance { get { return instance; } }

        public IBreakerCommanding BreakerCommanding { get; private set; }
        public IDERCommanding DERCommanding { get; set; }
    }
}
