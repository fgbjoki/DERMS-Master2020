using ClientUI.Events.OpenCommandingWindow;
using ClientUI.ViewModels.CommandingWindow.DERGroup.DER;
using Common.AbstractModel;
using Common.Communication;
using Common.ServiceInterfaces.UIAdapter.SummaryJobs;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.ViewModels.CommandingWindow.DERGroup
{
    public enum DERView
    {
        DERGroupView,
        GeneratorView,
        BatteryView
    }

    public class DERViewWrapper
    {
        public DERViewWrapper(DERView derView, string derViewName)
        {
            DERView = derView;
            DERViewName = derViewName;
        }

        public DERView DERView { get; set; }
        public string DERViewName { get; set; }
    }

    public struct DERGids
    {
        public DERGids(long generatorGid, long energyStorageGid)
        {
            GeneratorGid = generatorGid;
            EnergyStorageGid = energyStorageGid;
        }

        public long GeneratorGid { get; set; }
        public long EnergyStorageGid { get; set; }
    }

    public class DERGroupCommandingWindowViewModel : BaseCommandingViewModel
    {
        private DERGids derGids;

        private Dictionary<DERView, BaseDERCommandingEntityViewModel> viewModels;

        private DERViewWrapper selectedViewOption;

        private BaseDERCommandingEntityViewModel currentViewModel;

        private WCFClient<IDERGroupSummaryJob> derGroupSummary;

        public DERGroupCommandingWindowViewModel(DERGroupOpenCommandingWindowEventArgs args) : base("DER Commanding Window")
        {
            viewModels = new Dictionary<DERView, BaseDERCommandingEntityViewModel>();

            derGroupSummary = new WCFClient<IDERGroupSummaryJob>("uiAdapterDERGroupEndpoint");

            PopulateDERGids(args.GlobalId);
            InitializeDERViewOptions(args.DERView);
        }

        public ObservableCollection<DERViewWrapper> DERViewOptions { get; set; }

        public DERViewWrapper SelectedViewOption
        {
            get { return selectedViewOption; }
            set
            {
                if (selectedViewOption != value)
                {
                    SetProperty(ref selectedViewOption, value);
                    ResizeWindow(selectedViewOption);
                    ChangeViewModel(selectedViewOption);
                }
            }
        }

        public BaseDERCommandingEntityViewModel CurrentViewModel
        {
            get { return currentViewModel; }
            set { SetProperty(ref currentViewModel, value); }
        }

        private void ResizeWindow(DERViewWrapper selectedViewOption)
        {
            if (selectedViewOption.DERView == DERView.DERGroupView)
            {
                Width = 700;
            }
            else
            {
                Width = 500;
            }
        }

        private void InitializeDERViewOptions(DERView derView)
        {
            DERViewOptions = new ObservableCollection<DERViewWrapper>();

            if (derGids.EnergyStorageGid != 0)
            {
                DERViewOptions.Add(new DERViewWrapper(DERView.DERGroupView, "DER Group"));
            }

            if (derGids.GeneratorGid != 0)
            {
                DERViewOptions.Add(new DERViewWrapper(DERView.GeneratorView, "Generator"));
            }

            if (derGids.EnergyStorageGid != 0)
            {
                DERViewOptions.Add(new DERViewWrapper(DERView.BatteryView, "Energy storage"));
            }

            var selectedOption = DERViewOptions.FirstOrDefault(x => x.DERView == derView);
            if (selectedOption != null)
            {
                SelectedViewOption = selectedOption;
            }
        }

        private void ChangeViewModel(DERViewWrapper selectedViewOption)
        {
            BaseDERCommandingEntityViewModel selectedViewModel;
            if (!viewModels.TryGetValue(selectedViewOption.DERView, out selectedViewModel))
            {
                selectedViewModel = CreateViewModel(selectedViewOption.DERView);
                viewModels.Add(selectedViewOption.DERView, selectedViewModel);
            }

            CurrentViewModel?.StopFetchingData();
            CurrentViewModel = selectedViewModel;
            CurrentViewModel.StartFetchingData();
        }

        private BaseDERCommandingEntityViewModel CreateViewModel(DERView derViewType)
        {
            switch (derViewType)
            {
                case DERView.DERGroupView:
                    return new DERGroupCommandingViewModel(derGids, derGroupSummary);
                case DERView.GeneratorView:
                    return CreateGeneratorViewModel(derGids.GeneratorGid);
                case DERView.BatteryView:
                    return new DERBatteryCommandingViewModel(derGids.EnergyStorageGid, derGroupSummary);
                default:
                    throw new NotImplementedException();
            }
        }

        private BaseDERCommandingEntityViewModel CreateGeneratorViewModel(long generatorGid)
        {
            DMSType dmsType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(generatorGid);
            if (dmsType == DMSType.SOLARGENERATOR)
            {
                return new DERSolarPanelCommandingViewModel(generatorGid, derGroupSummary);
            }
            else if (dmsType == DMSType.WINDGENERATOR)
            {
                return new DERWindGeneratorCommandingViewModel(generatorGid, derGroupSummary);
            }

            return null;
        }

        private void PopulateDERGids(long entityGid)
        {
            try
            {
                var dto = derGroupSummary.Proxy.GetEntity(entityGid);
                derGids = new DERGids(dto.Generator.GlobalId, dto.EnergyStorage.GlobalId);
            }
            catch 
            {
                derGids = new DERGids(0, 0);
            }
        }

        protected override void StopProcessing()
        {
            CurrentViewModel?.StopFetchingData();
        }
    }
}
