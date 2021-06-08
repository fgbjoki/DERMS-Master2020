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
        private float maximumIncrease;
        private float maximumDecrease;

        private float setPoint;

        private bool setpointValid;

        protected CommandRequestDTO requestType;

        protected BaseCommandParameterViewModel(CommandRequestDTO requestType)
        {
            this.requestType = requestType;
            // TODO REMOVE THIS
            MaximumIncrease = 1500;
            MaximumDecrease = -1500;
        }

        public CommandRequestDTO RequestType { get { return requestType; } }

        public float MaximumIncrease
        {
            get { return maximumIncrease; }
            set
            {
                if (maximumIncrease != value)
                {
                    SetProperty(ref maximumIncrease, value);
                }
            }
        }

        public float MaximumDecrease
        {
            get { return maximumDecrease; }
            set
            {
                if (maximumDecrease != value)
                {
                    SetProperty(ref maximumDecrease, value);
                }
            }
        }

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
