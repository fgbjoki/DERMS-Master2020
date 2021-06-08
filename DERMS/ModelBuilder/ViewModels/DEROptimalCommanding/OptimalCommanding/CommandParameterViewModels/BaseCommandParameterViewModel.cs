using ClientUI.ViewModels.DEROptimalCommanding.OptimalCommanding.CommandParameterViewModels.Commands;
using Common.UIDataTransferObject.DEROptimalCommanding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClientUI.ViewModels.DEROptimalCommanding.OptimalCommanding.CommandParameterViewModels
{
    public abstract class BaseCommandParameterViewModel : BaseViewModel
    {
        private float setPoint;

        private bool setpointValid;

        protected CommandRequestDTO requestType;

        protected BaseCommandParameterViewModel(CommandRequestDTO requestType)
        {
            this.requestType = requestType;
        }

        public CommandRequestDTO RequestType { get { return requestType; } }

        public float SetPoint
        {
            get { return setPoint; }
            set
            {
                if (setPoint != value)
                {
                    SetProperty(ref setPoint, value);
                }
            }
        }

        public bool SetpointValid
        {
            get { return setpointValid; }
            set
            {
                SetProperty(ref setpointValid, value);
            }
        }
    }
}
