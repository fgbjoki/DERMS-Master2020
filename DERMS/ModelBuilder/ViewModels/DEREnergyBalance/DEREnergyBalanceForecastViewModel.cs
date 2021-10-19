using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientUI.SummaryCreator;
using System.Windows.Input;
using ClientUI.Common;
using System.Timers;
using System.Windows;
using ClientUI.CustomControls;
using ClientUI.Models.EnergyBalanceForecast;
using System.Threading;
using Common.Communication;
using Common.ServiceInterfaces.UIAdapter;
using Common.UIDataTransferObject.EnergyBalanceForecast;
using ClientUI.Common.MessageBox;

namespace ClientUI.ViewModels.DEREnergyBalance
{
    public class DEREnergyBalanceForecastViewModel : ContentViewModel
    {
        private WCFClient<IEnergyBalanceForecast> energyBalanceForecast;

        private float computeInterval;
        private float forecastInterval;
        private float costOfEnergyStorageUse;
        private float costOfEnergyImport;
        private float costOfGeneratorShutdown;

        private float totalCostOfEnergyStorageUsePerKWH;
        private float totalCostOfImportedEnergyUsePerKWH;
        private float totalCostOfGeneratorShutdownPerKWH;
        private float totalCost;

        private bool computeIntervalValid;
        private bool forecastIntervalValid;

        private Visibility waitingVisibility;
        private bool computing;
        private string waitingMessage;

        private Visibility resultVisibility;

        private int requestId;

        public DEREnergyBalanceForecastViewModel() : base("Optimal energy balance commanding", ContentType.DEREnergyBalanceCommanding)
        {
            ComputeCommand = new RelayCommand(ExecuteCompute, CanComputeExecute);

            ComputeInterval = 30;
            ForecastInterval = 15;

            CostOfEnergyImport = 0.5f;
            CostOfEnergyStorageUse = 0.05f;
            CostOfGeneratorShutDown = 1f;
            ComputeIntervalValid = true;
            ForecastIntervalValid = true;

            WaitingVisibility = Visibility.Hidden;
            ResultVisibility = Visibility.Hidden;
            Computing = false;

            EntityStates = new GIDMappedObservableCollection<DERState>();

            energyBalanceForecast = new WCFClient<IEnergyBalanceForecast>("uiEnergyBalanceForecast");
        }

        #region UI Properties

        public GIDMappedObservableCollection<DERState> EntityStates { get; set; }

        public float ComputeInterval
        {
            get { return computeInterval; }
            set
            {
                if (computeInterval != value)
                {
                    SetProperty(ref computeInterval, value);
                }
            }
        }

        public bool ComputeIntervalValid
        {
            get { return computeIntervalValid; }
            set
            {
                if (computeIntervalValid != value)
                {
                    SetProperty(ref computeIntervalValid, value);
                }
            }
        }

        public float ForecastInterval
        {
            get { return forecastInterval; }
            set
            {
                if (forecastInterval != value)
                {
                    SetProperty(ref forecastInterval, value);
                }
            }
        }

        public bool ForecastIntervalValid
        {
            get { return forecastIntervalValid; }
            set
            {
                if (forecastIntervalValid != value)
                {
                    SetProperty(ref forecastIntervalValid, value);
                }
            }
        }

        public float CostOfEnergyStorageUse
        {
            get { return costOfEnergyStorageUse; }
            set
            {
                if (costOfEnergyStorageUse != value)
                {
                    SetProperty(ref costOfEnergyStorageUse, value);
                }
            }
        }

        public float CostOfEnergyImport
        {
            get { return costOfEnergyImport; }
            set
            {
                if (costOfEnergyImport != value)
                {
                    SetProperty(ref costOfEnergyImport, value);
                }
            }
        }

        public float CostOfGeneratorShutDown
        {
            get { return costOfGeneratorShutdown; }
            set
            {
                if (costOfGeneratorShutdown != value)
                {
                    SetProperty(ref costOfGeneratorShutdown, value);
                }
            }
        }

        public Visibility WaitingVisibility
        {
            get { return waitingVisibility; }
            set
            {
                if (waitingVisibility != value)
                {
                    SetProperty(ref waitingVisibility, value);
                }
            }
        }

        public Visibility ResultVisibility
        {
            get { return resultVisibility; }
            set
            {
                if (resultVisibility != value)
                {
                    SetProperty(ref resultVisibility, value);
                }
            }
        }

        public bool Computing
        {
            get { return computing; }
            set
            {
                if (computing != value)
                {
                    SetProperty(ref computing, value);
                }
            }
        }


        public string WaitingMessage
        {
            get { return waitingMessage; }
            set
            {
                if (waitingMessage != value)
                {
                    SetProperty(ref waitingMessage, value);
                }
            }
        }

        public float TotalCost
        {
            get { return totalCost; }
            set
            {
                if (totalCost != value)
                {
                    SetProperty(ref totalCost, value);
                }
            }
        }

        public float TotalCostOfEnergyStorageUsePerKWH
        {
            get { return totalCostOfEnergyStorageUsePerKWH; }
            set
            {
                if (totalCostOfEnergyStorageUsePerKWH != value)
                {
                    SetProperty(ref totalCostOfEnergyStorageUsePerKWH, value);
                }
            }
        }

        public float TotalCostOfEnergyImportUsePerKWH
        {
            get { return totalCostOfImportedEnergyUsePerKWH; }
            set
            {
                if (totalCostOfImportedEnergyUsePerKWH != value)
                {
                    SetProperty(ref totalCostOfImportedEnergyUsePerKWH, value);
                }
            }
        }

        public float TotalCostOfGeneratorShutdownPerKWH
        {
            get { return totalCostOfGeneratorShutdownPerKWH; }
            set
            {
                if (totalCostOfGeneratorShutdownPerKWH != value)
                {
                    SetProperty(ref totalCostOfGeneratorShutdownPerKWH, value);
                }
            }
        }

        public ICommand ComputeCommand { get; set; }

        #endregion UI Properties

        private bool CanComputeExecute(object paramater)
        {
            return ComputeIntervalValid && ForecastIntervalValid;
        }

        private void ExecuteCompute(object parameter)
        {
            try
            {
                DomainParametersDTO domainParameter = new DomainParametersDTO()
                {
                    CalculatingTime = ComputeInterval,
                    CostOfEnergyStorageUsePerKWH = CostOfEnergyStorageUse,
                    CostOfGeneratorShutdownPerKWH = CostOfGeneratorShutDown,
                    CostOfImportedEnergyPerKWH = CostOfEnergyImport,
                    SimulationInterval = Convert.ToUInt64(ForecastInterval)
                };

                requestId = energyBalanceForecast.Proxy.Compute(domainParameter);
            }
            catch
            {
                MessageBoxCreator.Show("Feedback", "Service is not available at this moment, try again later.", MaterialDesignThemes.Wpf.PackIconKind.Error);
                return;
            }

            WaitingVisibility = Visibility.Visible;
            ResultVisibility = Visibility.Hidden;
            Computing = true;

            EntityStates.Clear();

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = ComputeInterval * 1000;
            timer.AutoReset = false;
            timer.Elapsed += FetchDataFromService;
            timer.Enabled = true;
            WaitingMessage = "Computing...";        
        }

        private void FetchDataFromService(object sender, ElapsedEventArgs e)
        {
            Computing = false;
            WaitingMessage = "Fetching result from service...";

            GetResultFromService();

            WaitingVisibility = Visibility.Hidden;
        }

        private void GetResultFromService()
        {
            int tries = 0;
            DERStatesSuggestionDTO dto = null;
            while (tries < 5)
            {
                try
                {
                    dto = energyBalanceForecast.Proxy.GetResults(requestId);
                    if (dto != null)
                    {
                        break;
                    }
                }
                catch
                {
                    tries++;
                }

                Thread.Sleep(2000);
            }

            if (tries == 5)
            {
                Application.Current.Dispatcher.Invoke((Action)delegate {
                    MessageBoxCreator.Show("Feedback", "Service couldn't create result in expected time. Try again.", MaterialDesignThemes.Wpf.PackIconKind.Error);
                });
                return;
            }

            PopulateData(dto);
            ResultVisibility = Visibility.Visible;
        }

        private void PopulateData(DERStatesSuggestionDTO dto)
        {
            TotalCostOfEnergyImportUsePerKWH = dto.CostOfEnergyUse.CostOfEnergyImport;
            TotalCostOfEnergyStorageUsePerKWH = dto.CostOfEnergyUse.CostOfEnergyStorageUse;
            TotalCostOfGeneratorShutdownPerKWH = dto.CostOfEnergyUse.CostOfGeneratorShutDown;

            TotalCost = TotalCostOfEnergyImportUsePerKWH + TotalCostOfEnergyStorageUsePerKWH + TotalCostOfGeneratorShutdownPerKWH;
            Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)(() => UpdateEntities(dto.DERStates)));

            
        }

        private void UpdateEntities(List<DERStateDTO> dtos)
        {
            foreach (var derState in dtos)
            {
                DERState newDerState = new DERState()
                {
                    GlobalId = derState.GlobalId,
                    Cost = derState.Cost,
                    EnergyUsed = derState.EnergyUsed,
                    IsEnergized = derState.IsEnergized,
                    Name = derState.Name
                };

                EntityStates.Add(newDerState);
            }
        }
    }
}
