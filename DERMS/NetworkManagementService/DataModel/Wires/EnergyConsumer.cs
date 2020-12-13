using NetworkManagementService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkManagementService.DataModel.Wires
{
    public class EnergyConsumer : ConductingEquipment
    {
        public EnergyConsumer(long globalId) : base(globalId)
        {

        }

        protected EnergyConsumer(EnergyConsumer copyObject) : base(copyObject)
        {

        }
        // TODO
    }
}
