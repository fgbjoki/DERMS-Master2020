using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.Models.NetworkModel.Generators
{
    public class WindGenerator : Generator
    {
        public float StartUpSpeed { get; set; }
        public float CutOutSpeed { get; set; }
        public float NominalSpeed { get; set; }
    }
}
