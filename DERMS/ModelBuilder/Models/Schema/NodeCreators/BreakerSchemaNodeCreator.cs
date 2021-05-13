using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ClientUI.Common;
using Common.UIDataTransferObject.Schema;
using ClientUI.Models.Schema.Nodes;
using ClientUI.Models.Schema.NodeCreators.BreakerCommands;

namespace ClientUI.Models.Schema.NodeCreators
{
    public class BreakerSchemaNodeCreator : SchemaNodeCreator
    {
        public BreakerSchemaNodeCreator() : base("")
        {
        }

        protected override void CustomConfiguration(SchemaNode node)
        {
            base.CustomConfiguration(node);
            node.Height /= 2;
            node.Width /= 2;
        }

        protected override ICommand GetOnClickCommand()
        {
            return null;
        }

        protected override void PopulateAdditionalProperties(SchemaNode node, SubSchemaNodeDTO dtoNode)
        {
            SubSchemaBreakerNodeDTO breakerNodeDTO = dtoNode as SubSchemaBreakerNodeDTO;
            SchemaBreakerNode breakerNode = node as SchemaBreakerNode;
            breakerNode.Closed = breakerNodeDTO.Closed;

            base.PopulateAdditionalProperties(node, dtoNode);
        }

        protected override List<ContextAction> CreateContextActions(SchemaNode node, SubSchemaNodeDTO dtoNode)
        {
            SchemaBreakerNode breakerNode = node as SchemaBreakerNode;

            return new List<ContextAction>()
            {
                new ContextAction("Open", new BreakerCommand(breakerNode, true)),
                new ContextAction("Close", new BreakerCommand(breakerNode, false))
            };
        }

        protected override SchemaNode InstantiateNode(long globalId, string imageUrl)
        {
            return new SchemaBreakerNode(globalId, imageUrl);
        }

    }
}
