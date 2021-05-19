using ClientUI.ViewModels.Summaries.NetworkSummary.EntityInformationViewModels;
using Common.AbstractModel;

namespace ClientUI.ViewModels.Summaries.NetworkSummary.Cache
{
    public interface IViewModelCache
    {
        BaseNetworkModelEntityInformationViewModel GetViewModel(DMSType dmsType);
    }
}