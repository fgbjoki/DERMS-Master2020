using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationEngine.Model.EnergyCalculations
{
    /// <summary>
    /// Represents anything that generates active power. (solar panel, wind generator, energy storage)
    /// </summary>
    public class EnergyGenerator : CalculationObject
    {
        public EnergyGenerator(long globalId) : base(globalId)
        {
        }
    }
}
