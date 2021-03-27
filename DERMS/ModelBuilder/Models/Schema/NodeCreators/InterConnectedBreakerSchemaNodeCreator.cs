using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClientUI.Models.Schema.NodeCreators
{
    public class InterConnectedBreakerSchemaNodeCreator : BreakerSchemaNodeCreator
    {
        public InterConnectedBreakerSchemaNodeCreator() : base()
        {
        }

        public override ICommand GetOnClickCommand()
        {
            return null;
        }
    }
}
