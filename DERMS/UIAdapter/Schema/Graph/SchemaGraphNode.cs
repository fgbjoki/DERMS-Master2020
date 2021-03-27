using Common.AbstractModel;
using System.Collections.Generic;

namespace UIAdapter.Schema.Graph
{
    public class SchemaGraphNode
    {
        public SchemaGraphNode(long globalId)
        {
            GlobalId = globalId;
            DMSType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(globalId);

            ChildBranches = new List<SchemaGraphBranch>();
        }

        public long GlobalId { get; set; }

        public DMSType DMSType { get; private set; }

        public bool IsEnergized { get; set; }

        public virtual bool DoesConduct { get { return IsEnergized; } }

        public ICollection<SchemaGraphBranch> ChildBranches { get; set; }

        public SchemaGraphBranch ParentBranch { get; set; }
    }
}
