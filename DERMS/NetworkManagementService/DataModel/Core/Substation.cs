using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkManagementService.DataModel.Core
{
    public class Substation : EquipmentContainer
    {
        public Substation(long globalId) : base(globalId)
        {

        }

        protected Substation(Substation copyObject) : base(copyObject)
        {

        }
        // TODO
    }
}
