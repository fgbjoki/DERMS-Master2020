using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.Models.Schema.Nodes
{
    public class SchemaBreakerNode : SchemaNode
    {
        private bool closed;

        public SchemaBreakerNode(long globalId, string imageSource) : base(globalId, imageSource)
        {
        }

        public bool Closed
        {
            get { return closed; }
            set
            {
                if (closed != value)
                {
                    SetProperty(ref closed, value);
                }
            }
        }
    }
}
