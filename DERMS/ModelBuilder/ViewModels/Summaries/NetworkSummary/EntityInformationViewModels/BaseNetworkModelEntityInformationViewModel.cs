using ClientUI.Models;
using ClientUI.Models.NetworkModel;
using Common.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.ViewModels.Summaries.NetworkSummary.EntityInformationViewModels
{
    public abstract class BaseNetworkModelEntityInformationViewModel : BaseViewModel
    {
        protected BaseNetworkModelEntityInformationViewModel()
        {
            // dependency injection of wcf service
        }

        public void PopulateFields(long entityGid)
        {
            IdentifiedObject entity = GetEntityFromService(entityGid);
            PopulateFields(entity);
        }

        protected abstract void PopulateFields(IdentifiedObject entity);

        private IdentifiedObject GetEntityFromService(long entityGid)
        {
            // TODO CHANGE THIS
            return new EnergyStorage();
        }
    }
}
