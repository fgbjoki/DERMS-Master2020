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
        private Timer refreshContentTimer;

        public EntityCommandingViewModel(string title) : base(title)
        {
            refreshContentTimer = new Timer();
            refreshContentTimer.AutoReset = true;
            refreshContentTimer.Interval = 5 * 1000;
            refreshContentTimer.Elapsed += FetchContent;
            refreshContentTimer.Enabled = true;
        }

        protected void FetchContent(object sender, ElapsedEventArgs e)
        {
            refreshContentTimer.Enabled = false;
            RefreshContent();
            refreshContentTimer.Enabled = true;
        }

        protected abstract void RefreshContent();
    }
}
