using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientUI.ViewModels.CommandingWindow.DERGroup.DER;

namespace ClientUI.Models.DERGroup.CommandingWindow.DERGroup
{
    public class WindGenerator : Generator
    {
        public WindGenerator() : base(GeneratorType.Wind, "../../Resources/DER/windturbine1.png")
        {
        }
    }
}
