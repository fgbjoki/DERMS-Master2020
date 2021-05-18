using ClientUI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ClientUI.Models.Schema.NodeCreators
{
    public class NonClickableSchemaNodeCreator : SchemaNodeCreator
    {
        public NonClickableSchemaNodeCreator(string imageUrl = "") : base(imageUrl)
        {
        }

        protected override ICommand GetOnClickCommand()
        {
            return null;
        }
    }
}
