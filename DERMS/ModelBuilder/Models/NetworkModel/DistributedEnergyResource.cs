using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.Models.NetworkModel
{
    public class DistributedEnergyResource : IdentifiedObject
    {
        public DistributedEnergyResource()
        {
            Measurements = new List<Measurement>(1);
        }

        public float NominalActivePower { get; set; }

        public List<Measurement> Measurements { get; set; }
    }
}
