using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.ViewModels.DEROptimalCommanding.OptimalCommanding
{
    public enum OptimalCommandingType
    {
        NominalPower,
        Reserve
    }

    public class CommandingTypeOption
    {
        public CommandingTypeOption(string name, OptimalCommandingType commandingType)
        {
            Name = name;
            OptimalCommandingType = commandingType;
        }

        public string Name { get; set; }

        public OptimalCommandingType OptimalCommandingType { get; set; }
    }
}
