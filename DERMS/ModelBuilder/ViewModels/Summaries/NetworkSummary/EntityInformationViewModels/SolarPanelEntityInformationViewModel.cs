using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.UIDataTransferObject.NetworkModel;

namespace ClientUI.ViewModels.Summaries.NetworkSummary.EntityInformationViewModels
{
    public class SolarPanelEntityInformationViewModel : DistributedEnergyResourceEntityInformationViewModel
    {
        public SolarPanelEntityInformationViewModel()
        {
            MeasurementViewModels.Add(new AnalogMeasurementEntityInformationViewModel());
        }
    }
}
