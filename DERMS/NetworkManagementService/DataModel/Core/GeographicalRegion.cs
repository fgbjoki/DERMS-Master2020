using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkManagementService.DataModel.Core
{
    public class GeographicalRegion : IdentifiedObject
    {
        public GeographicalRegion(long globalId) : base(globalId)
        {

        }

        protected GeographicalRegion(GeographicalRegion copyObject) : base(copyObject)
        {

        }
        // TODO
    }
}
