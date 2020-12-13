using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkManagementService.DataModel.Core
{
    public class SubGeographicalRegion : IdentifiedObject
    {
        public SubGeographicalRegion(long globalId) : base(globalId)
        {

        }

        protected SubGeographicalRegion(SubGeographicalRegion copyObject) : base(copyObject)
        {

        }
        // TODO
    }
}
