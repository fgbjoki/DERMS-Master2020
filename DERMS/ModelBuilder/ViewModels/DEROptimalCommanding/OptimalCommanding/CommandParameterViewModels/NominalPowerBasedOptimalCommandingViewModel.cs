using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.UIDataTransferObject.DEROptimalCommanding;

namespace ClientUI.ViewModels.DEROptimalCommanding.OptimalCommanding.CommandParameterViewModels
{
    public class NominalPowerBasedOptimalCommandingViewModel : BaseCommandParameterViewModel
    {
        public NominalPowerBasedOptimalCommandingViewModel() : base(CommandRequestDTO.NominalPower)
        {
        }
    }
}
