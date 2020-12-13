using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkManagementService.DataModel.Core
{
    public class EquipmentContainer : ConnectivityNodeContainer
    {
        public EquipmentContainer(long globalId) : base(globalId)
        {

        }

        protected EquipmentContainer(EquipmentContainer copyObject) : base(copyObject)
        {

        }
        // TODO
    }
}
