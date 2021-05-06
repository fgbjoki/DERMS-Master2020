using ClientUI.ViewModels.CommandingWindow.DERGroup.DER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.Models.DERGroup.CommandingWindow.DERGroup
{
    public class Generator : DistributedEnergyResource
    {
        public Generator(GeneratorType generatorType, string imageUrl) : base(imageUrl)
        {
            GeneratorType = generatorType;
        }

        public GeneratorType GeneratorType { get; protected set; }
    }
}
