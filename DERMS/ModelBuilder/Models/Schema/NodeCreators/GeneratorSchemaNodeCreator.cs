using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClientUI.Models.Schema.NodeCreators
{
    public class GeneratorSchemaNodeCreator : SchemaNodeCreator
    {
        public GeneratorSchemaNodeCreator(string imageUrl= "") : base(imageUrl)
        {
        }

        protected override ICommand GetOnClickCommand()
        {
            // TODO OPEN COMMANDING WINDOW
            return null;
        }
    }
}
