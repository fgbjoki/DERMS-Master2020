using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.UIDataTransferObject;
using Common.UIDataTransferObject.DERGroup;

namespace ClientUI.Models.DERGroup.CommandingWindow.DERGroup
{
    public class EnergyStorage : DistributedEnergyResource
    {
        private static readonly string imageDirectory = "../../Resources/DER/EnergyStorage";

        private float stateOfCharge;
        private float capacity;

        public EnergyStorage() : base("")
        {
            ChangeImage();
        }

        public float Capacity
        {
            get { return capacity; }
            protected set
            {
                if (capacity != value)
                {
                    SetProperty(ref capacity, value);
                }
            }
        }

        public float StateOfCharge
        {
            get { return stateOfCharge; }
            protected set
            {
                float newStateOfCharge = value * 100;
                if (stateOfCharge != newStateOfCharge)
                {
                    SetProperty(ref stateOfCharge, newStateOfCharge);
                    ChangeImage();
                }
            }
        }

        private void ChangeImage()
        {
            if (stateOfCharge > 75)
            {
                ImageSource = $"{imageDirectory}/100.png";
            }
            else if (stateOfCharge > 50)
            {
                ImageSource = $"{imageDirectory}/75.png";
            }
            else if (stateOfCharge > 25)
            {
                ImageSource = $"{imageDirectory}/50.png";
            }
            else
            {
                ImageSource = $"{imageDirectory}/25.png";
            }
        }

        protected override void UpdateProperties(IdentifiedObjectDTO entity)
        {
            base.UpdateProperties(entity);
            DERGroupEnergyStorageSummaryDTO dto = entity as DERGroupEnergyStorageSummaryDTO;
            if (dto == null)
            {
                return;
            }

            StateOfCharge = dto.StateOfCharge;
            Capacity = dto.Capacity;
        }
    }
}
