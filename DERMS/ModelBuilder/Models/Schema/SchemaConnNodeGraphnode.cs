using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ClientUI.Models.Schema
{
    public class SchemaConnNodeGraphNode : SchemaNode
    {
        private static SolidColorBrush black = new SolidColorBrush(Colors.Black);

        public SchemaConnNodeGraphNode(long globalId, string imageSource) : base(globalId, imageSource)
        {
        }

        public override SolidColorBrush Outline
        {
            get
            {
                return black;
            }

            set
            {
                
            }
        }
    }
}
