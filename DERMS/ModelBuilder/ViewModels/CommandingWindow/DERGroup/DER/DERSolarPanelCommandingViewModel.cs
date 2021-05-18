using Common.AbstractModel;
using Common.Communication;
using Common.ServiceInterfaces.UIAdapter.SummaryJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.ViewModels.CommandingWindow.DERGroup.DER
{
    public class DERSolarPanelCommandingViewModel : BaseDERGeneratorCommandingViewModel
    {
        public DERSolarPanelCommandingViewModel(long derGlobalId, WCFClient<IDERGroupSummaryJob> derGroupSummary) : base(derGlobalId, derGroupSummary, "../../Resources/DER/solarPanel.png")
        {
            
        }      
        
    }
}
