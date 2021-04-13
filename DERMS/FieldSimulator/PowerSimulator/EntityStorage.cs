using Common.AbstractModel;
using FieldSimulator.PowerSimulator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldSimulator.PowerSimulator
{
    public class EntityStorage
    {
        public Dictionary<DMSType, Dictionary<long, IdentifiedObject>> Storage { get; private set; } = new Dictionary<DMSType, Dictionary<long, IdentifiedObject>>();
    }
}
