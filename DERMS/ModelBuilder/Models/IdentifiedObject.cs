using Common.AbstractModel;
using Common.UIDataTransferObject;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.Models
{
    public class IdentifiedObject : BindableBase
    {
        private long globalId;
        private string name;
        private string description;
        private string mrid;

        public long GlobalId
        {
            get { return globalId; }
            set
            {
                SetProperty(ref globalId, value);
                DMSType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(value);
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    SetProperty(ref name, value);
                }
            }
        }

        public DMSType DMSType
        {
            get;
            private set;
        }

        public string Description
        {
            get { return description; }
            set
            {
                if (description != value)
                {
                    SetProperty(ref description, value);
                }
            }
        }

        public string MRID
        {
            get { return mrid; }
            set
            {
                if (mrid != value)
                {
                    SetProperty(ref mrid, value);
                }
            }
        }

        public void Update(IdentifiedObject entity)
        {
            UpdateProperties(entity);
        }

        protected virtual void UpdateProperties(IdentifiedObject entity)
        {
            this.Name = entity.Name;
            this.Description = entity.Description;
        }

        public void Update(IdentifiedObjectDTO entity)
        {
            UpdateProperties(entity);
        }

        protected virtual void UpdateProperties(IdentifiedObjectDTO entity)
        {
            this.Name = entity.Name;
            this.GlobalId = entity.GlobalId;
            this.Description = entity.Description;
        }
    }
}
