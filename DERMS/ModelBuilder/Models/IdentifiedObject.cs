using Common.UIDataTransferObject;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.Models
{
    public abstract class IdentifiedObject : BindableBase
    {
        private long globalId;
        private string name;

        public long GlobalId
        {
            get { return globalId; }
            set { SetProperty(ref globalId, value); }
        }

        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        public void Update(IdentifiedObject entity)
        {
            UpdateProperties(entity);
        }

        protected virtual void UpdateProperties(IdentifiedObject entity)
        {
            this.Name = entity.Name;
        }

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
