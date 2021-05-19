using ClientUI.ViewModels.Summaries.NetworkSummary.EntityInformationViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientUI.Models;
using ClientUI.Models.NetworkModel;
using Common.UIDataTransferObject;
using Common.UIDataTransferObject.NetworkModel;
using Common.UIDataTransferObject.NetworkModel.ConductingEquipment;

namespace ClientUI.ViewModels.Summaries.NetworkSummary.EntityInformationViewModels
{
    public class EnergyStorageEntityInformationViewModel : DistributedEnergyResourceEntityInformationViewModel
    {
        private float capacity;

        public EnergyStorageEntityInformationViewModel()
        {
            MeasurementViewModels.Add(new AnalogMeasurementEntityInformationViewModel());
            MeasurementViewModels.Add(new AnalogMeasurementEntityInformationViewModel());
        }

        public override void PopulateFields(NetworkModelEntityDTO entity)
        {
            base.PopulateFields(entity);

            EnergyStorageDTO dto = entity as EnergyStorageDTO;
            Capacity = dto.Capacity;
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
    }
}
