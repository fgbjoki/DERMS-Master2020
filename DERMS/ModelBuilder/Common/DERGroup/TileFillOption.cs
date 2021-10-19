using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.Common.DERGroup
{
    public enum TileFillEnum
    {
        //ActivePower,
        EnergyStateOfCharge
    }

    public class TileFillOption
    {
        public TileFillOption(string name, TileFillEnum tileFill)
        {
            Name = name;
            TileFill = tileFill;
        }

        public string Name { get; set; }
        public TileFillEnum TileFill { get; set; }
    }
}
