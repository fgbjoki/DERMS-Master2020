using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.Models.EnergyBalanceForecast
{
    public class DERState : IdentifiedObject
    {
        public float EnergyUsed { get; set; }

        public bool IsEnergized { get; set; }

        public float Cost { get; set; }
    }
}
