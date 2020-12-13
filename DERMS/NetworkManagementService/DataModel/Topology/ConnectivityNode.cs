using NetworkManagementService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkManagementService.DataModel.Topology
{
    public class ConnectivityNode : IdentifiedObject
    {
        public ConnectivityNode(long globalId) : base(globalId)
        {

        }

        protected ConnectivityNode(ConnectivityNode copyObject) : base(copyObject)
        {

        }
        // TODO
    }
}
