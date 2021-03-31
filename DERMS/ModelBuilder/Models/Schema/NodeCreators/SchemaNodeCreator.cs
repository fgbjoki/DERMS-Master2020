using Common.UIDataTransferObject.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClientUI.Models.Schema.NodeCreators
{
    public abstract class SchemaNodeCreator
    {
        private string imageUrl;

        protected SchemaNodeCreator(string imageUrl)
        {
            this.imageUrl = imageUrl;
        }

        public SchemaNode CreateNode(SubSchemaNodeDTO dtoNode)
        {
            SchemaNode node = InstantiateNode(dtoNode.GlobalId, imageUrl);
            node.DoesConduct = dtoNode.DoesConduct;
            node.Energized = dtoNode.IsEnergized;
            node.OnDoubleClick = GetOnClickCommand();

            CustomConfiguration(node);

            return node;
        }

        public abstract ICommand GetOnClickCommand();

        public virtual SchemaNode InstantiateNode(long globalId, string imageUrl)
        {
            return new SchemaNode(globalId, imageUrl);
        }

        public virtual void CustomConfiguration(SchemaNode node)
        {

        }
    }
}
