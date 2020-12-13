using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkManagementService.DataModel.Core
{
    public class Terminal : IdentifiedObject
    {
        public Terminal(long globalId) : base(globalId)
        {

        }

        protected Terminal(Terminal copyObject) : base(copyObject)
        {

        }
        // TODO
    }
}
