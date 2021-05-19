using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.ViewModels.Summaries.NetworkSummary.EntityInformationViewModels
{
    public class EnergySourceEntityInformationViewModel : ConductingEquipmentEntityInformationViewModel
    {
        public EnergySourceEntityInformationViewModel()
        {
            MeasurementViewModels.Add(new AnalogMeasurementEntityInformationViewModel());
        }
    }
}
