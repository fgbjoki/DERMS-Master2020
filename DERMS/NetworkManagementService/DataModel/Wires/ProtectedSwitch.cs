using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkManagementService.DataModel.Wires
{
    public class ProtectedSwitch : Switch
    {
        public ProtectedSwitch(long globalId) : base(globalId)
        {

        }

        protected ProtectedSwitch(ProtectedSwitch copyObject) : base(copyObject)
        {

        }
        // TODO
    }
}
