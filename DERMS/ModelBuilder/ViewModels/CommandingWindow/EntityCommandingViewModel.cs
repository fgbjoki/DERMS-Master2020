using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ClientUI.ViewModels.CommandingWindow
{
    public abstract class EntityCommandingViewModel : BaseCommandingViewModel
    {
        protected Timer refreshContentTimer;

        public EntityCommandingViewModel(string title) : base(title)
        {
            refreshContentTimer = new Timer();
            refreshContentTimer.AutoReset = true;
            refreshContentTimer.Interval = 2 * 1000;
            refreshContentTimer.Elapsed += FetchContent;
            refreshContentTimer.Enabled = true;
        }

        public void StopFetchingData()
        {
            refreshContentTimer.Enabled = false;
        }
        public void StartFetchingData()
        {
            FetchContent();
            refreshContentTimer.Enabled = true;
        }

        protected void FetchContent(object sender, ElapsedEventArgs e)
        {
            refreshContentTimer.Enabled = false;
            FetchContent();
            refreshContentTimer.Enabled = true;
        }

        protected abstract void FetchContent();

        protected override void StopProcessing()
        {
            StopFetchingData();
        }
    }
}
