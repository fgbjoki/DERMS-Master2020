using Common.Communication;
using Common.ServiceInterfaces.UIAdapter.SummaryJobs;
using Common.UIDataTransferObject.DERGroup;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.ViewModels.CommandingWindow.DERGroup
{
    public abstract class BaseDERCommandingEntityViewModel : EntityCommandingViewModel
    {
        protected long entityGid;
        protected WCFClient<IDERGroupSummaryJob> derGroupSummary;

        public BaseDERCommandingEntityViewModel(long entityGid, WCFClient<IDERGroupSummaryJob> derGroupSummary) : base("")
        {
            this.entityGid = entityGid;
            this.derGroupSummary = derGroupSummary;
        }

        protected override void FetchContent()
        {
            DERGroupSummaryDTO dto = null;
            try
            {
                dto = derGroupSummary.Proxy.GetEntity(entityGid);
                PopulateObject(dto);
            }
            catch { }
        }

        protected abstract void PopulateObject(DERGroupSummaryDTO dto);
    }
}
