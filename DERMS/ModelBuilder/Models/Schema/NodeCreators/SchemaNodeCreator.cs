using ClientUI.Common;
using Common.UIDataTransferObject.Schema;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            PopulateAdditionalProperties(node, dtoNode);
            CustomConfiguration(node);

            node.ContextActions = new ObservableCollection<ContextAction>(CreateContextActions(node, dtoNode));

            return node;
        }

        protected abstract ICommand GetOnClickCommand();

        protected virtual SchemaNode InstantiateNode(long globalId, string imageUrl)
        {
            return new SchemaNode(globalId, imageUrl);
        }

        protected virtual void CustomConfiguration(SchemaNode node)
        {

        }

        protected virtual List<ContextAction> CreateContextActions(SchemaNode node, SubSchemaNodeDTO dtoNode)
        {
            return new List<ContextAction>(0);
        }

        protected virtual void PopulateAdditionalProperties(SchemaNode node, SubSchemaNodeDTO dtoNode)
        {
        }
    }
}
