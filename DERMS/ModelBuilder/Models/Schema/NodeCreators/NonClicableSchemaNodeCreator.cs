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
    public class NonClicableSchemaNodeCreator : SchemaNodeCreator
    {
        public NonClicableSchemaNodeCreator() : base(null)
        {
        }

        public override ICommand GetOnClickCommand()
        {
            return null;
        }
    }
}
