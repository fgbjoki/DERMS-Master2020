using ClientUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.CustomControls.DEROptimalFeedbackIcons
{
    public class FeedbackIconCollectionViewModel : BaseViewModel
    {
        private bool valid;
        private bool error;
        private bool highLoad;
        private bool batteryLow;

        public bool Valid
        {
            get { return valid; }
            set
            {
                if (valid != value)
                {
                    SetProperty(ref valid, value);
                }
            }
        }

        public bool Error
        {
            get { return error; }
            set
            {
                if (error != value)
                {
                    SetProperty(ref error, value);
                }
            }
        }

        public bool HighLoad
        {
            get { return highLoad; }
            set
            {
                if (highLoad != value)
                {
                    SetProperty(ref highLoad, value);
                }
            }
        }

        public bool BatteryLow
        {
            get { return batteryLow; }
            set
            {
                if (batteryLow != value)
                {
                    SetProperty(ref batteryLow, value);
                }
            }
        }
    }
}
