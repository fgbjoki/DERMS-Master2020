using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.UIDataTransferObject.DEROptimalCommanding;

namespace ClientUI.ViewModels.DEROptimalCommanding.OptimalCommanding.CommandSummaryViewModels
{
    public class ReserveBasedCommandSummaryViewModel : BaseCommandSummaryViewModel
    {
        protected override BaseSummarySuggestedValueViewModel CreateSuggestedSample(SuggestedCommand suggestedCommand)
        {
            ReserveSuggestedValueViewModel suggestedValue = new ReserveSuggestedValueViewModel();
            suggestedValue.PopulateFields(suggestedCommand);

            return suggestedValue;
        }
    }
}
