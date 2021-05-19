using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.UIDataTransferObject.NetworkModel;
using Common.UIDataTransferObject.NetworkModel.ConductingEquipment;

namespace ClientUI.ViewModels.Summaries.NetworkSummary.EntityInformationViewModels
{
    public abstract class ConductingEquipmentEntityInformationViewModel : BaseNetworkModelEntityInformationViewModel
    {
        private MeasurementEntityInformationViewModel selectedMeasurement;

        public ConductingEquipmentEntityInformationViewModel()
        {
            MeasurementViewModels = new ObservableCollection<MeasurementEntityInformationViewModel>();
        }

        public ObservableCollection<MeasurementEntityInformationViewModel> MeasurementViewModels { get; set; }

        public MeasurementEntityInformationViewModel SelectedMeasurement
        {
            get { return selectedMeasurement; }
            set
            {
                if (selectedMeasurement != value)
                {
                    SetProperty(ref selectedMeasurement, value);
                }
            }
        }

        public override void PopulateFields(NetworkModelEntityDTO entity)
        {
            base.PopulateFields(entity);

            ConductingEquipmentDTO dto = entity as ConductingEquipmentDTO;

            for (int i = 0; i < dto.Measurements.Count; i++)
            {
                MeasurementViewModels[i].PopulateFields(dto.Measurements[i]);
            }

            for (int i = dto.Measurements.Count; i < MeasurementViewModels.Count; i++)
            {
                MeasurementViewModels[i].Visibility = System.Windows.Visibility.Hidden;
            }
        }
    }
}
