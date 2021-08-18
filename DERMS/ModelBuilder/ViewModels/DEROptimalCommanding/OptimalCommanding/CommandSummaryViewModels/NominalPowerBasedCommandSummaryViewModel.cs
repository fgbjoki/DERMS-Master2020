using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.UIDataTransferObject.DEROptimalCommanding;

namespace ClientUI.ViewModels.DEROptimalCommanding.OptimalCommanding.CommandSummaryViewModels
{
    public class NominalPowerBasedCommandSummaryViewModel : BaseCommandSummaryViewModel
    {
        protected override BaseSummarySuggestedValueViewModel CreateSuggestedSample(SuggestedCommand suggestedCommand)
        {
            NominalPowerBasedSuggestedValueViewModel suggestedItem = new NominalPowerBasedSuggestedValueViewModel();
            suggestedItem.PopulateFields(suggestedCommand);

            return suggestedItem;
        }
    }
}
