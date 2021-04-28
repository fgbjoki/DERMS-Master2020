using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ClientUI.Models.Schema.NodeCreators
{
    public class ConnectivityNodeSchemaNodeCreator : NonClicableSchemaNodeCreator
    {
        protected override void CustomConfiguration(SchemaNode node)
        {
            base.CustomConfiguration(node);

            node.Width = 7;
            node.Height = 7;
            node.Outline = new SolidColorBrush(Colors.Black);
        }

        protected override SchemaNode InstantiateNode(long globalId, string imageUrl)
        {
            return new SchemaConnNodeGraphNode(globalId, imageUrl);
        }
    }
}
