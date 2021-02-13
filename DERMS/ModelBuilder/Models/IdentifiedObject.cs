using Common.UIDataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.Models
{
    public abstract class IdentifiedObject
    {
        public long GlobalId { get; set; }
        public string Name { get; set; }

        public void Update(IdentifiedObjectDTO entity)
        {
            UpdateProperties(entity);
        }

        protected virtual void UpdateProperties(IdentifiedObjectDTO entity)
        {
            this.Name = entity.Name;
            this.GlobalId = entity.GlobalId;
        }
    }
}
