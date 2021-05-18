using ClientUI.ViewModels.Summaries.NetworkSummary.EntityInformationViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientUI.Models;
using ClientUI.Models.NetworkModel;

namespace ClientUI.ViewModels.Summaries.NetworkSummary.EntityInformationViewModels
{
    public class EnergyStorageEntityInformationViewModel : BaseNetworkModelEntityInformationViewModel
    {
        protected override void PopulateFields(IdentifiedObject entity)
        {
            EnergyStorage energyStorage = entity as EnergyStorage;
        }
    }
}
