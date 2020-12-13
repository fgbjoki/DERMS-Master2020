using NetworkManagementService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkManagementService.DataModel.Wires
{
    public class Switch : ConductingEquipment
    {
        public Switch(long globalId) : base(globalId)
        {

        }

        protected Switch(Switch copyObject) : base(copyObject)
        {

        }
        // TODO
    }
}
