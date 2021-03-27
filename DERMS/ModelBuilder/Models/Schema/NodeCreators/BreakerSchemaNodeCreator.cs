using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Common.UIDataTransferObject.Schema;

namespace ClientUI.Models.Schema.NodeCreators
{
    public class BreakerSchemaNodeCreator : SchemaNodeCreator
    {
        public BreakerSchemaNodeCreator() : base("../ModelBuilder/Resources/switch-symbol-circuit-electric-97624.png")
        {
        }

        public override ICommand GetOnClickCommand()
        {
            return null;
        }
    }
}
