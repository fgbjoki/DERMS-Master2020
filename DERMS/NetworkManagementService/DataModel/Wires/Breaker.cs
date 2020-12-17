using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkManagementService.DataModel.Wires
{
    public class Breaker : ProtectedSwitch
    {
        public Breaker(long globalId) : base(globalId)
        {

        }

        protected Breaker(Breaker copyObject) : base(copyObject)
        {

        }
        // TODO
    }
}
