using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.ViewModels.Summaries.NetworkSummary.EntityInformationViewModels
{
    public class EnergyConsumerEntityInformationViewModel : ConductingEquipmentEntityInformationViewModel
    {
        public EnergyConsumerEntityInformationViewModel()
        {
            MeasurementViewModels.Add(new AnalogMeasurementEntityInformationViewModel());
        }
    }
}
