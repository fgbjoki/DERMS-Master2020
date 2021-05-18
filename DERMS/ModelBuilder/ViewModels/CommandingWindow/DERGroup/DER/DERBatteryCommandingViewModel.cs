using Common.Communication;
using Common.ServiceInterfaces.UIAdapter.SummaryJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.UIDataTransferObject.DERGroup;

namespace ClientUI.ViewModels.CommandingWindow.DERGroup.DER
{
    public class DERBatteryCommandingViewModel : DERInformationViewModel
    {
        private static readonly string imageDirectory = "../../Resources/DER/EnergyStorage";
        private float capacity;
        private float stateOfCharge;

        public DERBatteryCommandingViewModel(long derGlobalId, WCFClient<IDERGroupSummaryJob> derGroupSummary) : base(derGlobalId, derGroupSummary)
        {
            ChangeEnergyImage();
        }

        public float Capacity
        {
            get { return capacity; }
            set
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
            set
            {
                float newStateOfCharge = value * 100;
                if (stateOfCharge != newStateOfCharge)
                {
                    SetProperty(ref stateOfCharge, newStateOfCharge);
                    ChangeEnergyImage();
                }
            }
        }

        protected override void PopulateObject(DERGroupSummaryDTO dto)
        {
            DERGroupEnergyStorageSummaryDTO energyStorage = dto.EnergyStorage;
            Name = energyStorage.Name;
            NominalPower = energyStorage.NominalPower;
            ActivePower = energyStorage.ActivePower;
            Capacity = energyStorage.Capacity;
            StateOfCharge = energyStorage.StateOfCharge;
        }

        private void ChangeEnergyImage()
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
    }
}
