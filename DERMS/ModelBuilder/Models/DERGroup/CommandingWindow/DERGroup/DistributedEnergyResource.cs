using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.UIDataTransferObject;

namespace ClientUI.Models.DERGroup.CommandingWindow.DERGroup
{
    public class DistributedEnergyResource : IdentifiedObject
    {
        private float activePower;
        private string imageUrl;

        public DistributedEnergyResource(string imageUrl)
        {
            this.imageUrl = imageUrl;
        }

        public float ActivePower
        {
            get { return activePower; }
            set
            {
                if (activePower != value)
                {
                    SetProperty(ref activePower, value);
                }
            }
        }

        public string ImageSource
        {
            get { return imageUrl; }
            set
            {
                if (imageUrl != value)
                {
                    SetProperty(ref imageUrl, value);
                }
            }
        }

        protected override void UpdateProperties(IdentifiedObjectDTO entity)
        {
            base.UpdateProperties(entity);
            DistributedEnergyResourceDTO dto = entity as DistributedEnergyResourceDTO;
            if (dto == null)
            {
                return;
            }

            ActivePower = dto.ActivePower;
        }
    }
}
