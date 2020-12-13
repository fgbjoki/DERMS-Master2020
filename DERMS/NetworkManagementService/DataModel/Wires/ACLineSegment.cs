using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkManagementService.DataModel.Wires
{
    public class ACLineSegment : Conductor
    {
        public ACLineSegment(long globalId) : base(globalId)
        {

        }

        protected ACLineSegment(ACLineSegment copyObject) : base(copyObject)
        {

        }
        // TODO
    }
}
