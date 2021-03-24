using System.Collections.Generic;

namespace Common.DataTransferObjects.CalculationEngine
{
    public class SchemaGraphChanged
    {
        public SchemaGraphChanged()
        {

        }

        public SchemaGraphChanged(Dictionary<long, List<long>> parentChildBranches, long energySourceGid, long interConnectedBreakerGid)
        {
            EnergySourceGid = energySourceGid;
            ParentChildBranches = parentChildBranches;
            InterConnectedBreakerGid = interConnectedBreakerGid;
        }

        public long InterConnectedBreakerGid { get; set; }

        public long EnergySourceGid { get; set; }

        public Dictionary<long, List<long>> ParentChildBranches { get; set; }
    }
}
