using Common.ComponentStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIAdapter.Model
{
    public class RemotePoint : IdentifiedObject
    {
        public RemotePoint(long globalId) : base(globalId)
        {
        }

        public string Name { get; set; }

        public string CurrentValue { get; protected set; }

        public int Address { get; set; }

        public void PopulateValueField(float value)
        {
            CurrentValue = value.ToString();
        }

        public void PopulateValueField(int value)
        {
            CurrentValue = value.ToString();
        }
    }
}
