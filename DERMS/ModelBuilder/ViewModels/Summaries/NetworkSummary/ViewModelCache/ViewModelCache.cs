using ClientUI.ViewModels.Summaries.NetworkSummary.EntityInformationViewModels;
using Common.AbstractModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.ViewModels.Summaries.NetworkSummary.Cache
{
    public class ViewModelCache : IViewModelCache
    {
        private Dictionary<DMSType, BaseNetworkModelEntityInformationViewModel> viewModels;

        public ViewModelCache()
        {
            InitializeViewModels();
        }

        public BaseNetworkModelEntityInformationViewModel GetViewModel(DMSType dmsType)
        {
            BaseNetworkModelEntityInformationViewModel viewModel;

            viewModels.TryGetValue(dmsType, out viewModel);

            return viewModel;
        }

        private void InitializeViewModels()
        {
            viewModels = new Dictionary<DMSType, BaseNetworkModelEntityInformationViewModel>()
            {
                { DMSType.ENERGYSTORAGE, new EnergyStorageEntityInformationViewModel()      },
                { DMSType.BREAKER, new BreakerEntityInformationViewModel()                  },
                { DMSType.ENERGYCONSUMER, new EnergyConsumerEntityInformationViewModel()    },
                { DMSType.ENERGYSOURCE, new EnergySourceEntityInformationViewModel()        },
                { DMSType.SOLARGENERATOR, new SolarPanelEntityInformationViewModel()        },
                { DMSType.WINDGENERATOR, new WindGeneratorEntityInformationViewModel()      }
            };
        }
    }
}
