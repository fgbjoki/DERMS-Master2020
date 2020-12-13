using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkManagementService.DataModel.Core
{
    public class ConnectivityNodeContainer : PowerSystemResource
    {
        public ConnectivityNodeContainer(long globalId) : base(globalId)
        {

        }

        protected ConnectivityNodeContainer(ConnectivityNodeContainer copyObject) : base(copyObject)
        {

        }
        // TODO
    }
}
