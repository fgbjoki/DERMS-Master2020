using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientUI.ViewModels.CommandingWindow.DERGroup.DER;

namespace ClientUI.Models.DERGroup.CommandingWindow.DERGroup
{
    public class SolarPanel : Generator
    {
        public SolarPanel() : base(GeneratorType.Solar, "../../Resources/DER/solarPanel.png")
        {
        }
    }
}
