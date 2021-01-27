using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIAdapter.Model
{
    public class AnalogRemotePoint : RemotePoint
    {
        private float value;

        public AnalogRemotePoint(long globalId) : base(globalId)
        {
        }

        public float Value
        {
            get { return value; }
            set
            {
                this.value = value;
                PopulateValueField(value);
            }
        }

    }
}
