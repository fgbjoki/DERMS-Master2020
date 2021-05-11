using ClientUI.Common;
using ClientUI.Events.Schema;
using ClientUI.SummaryCreator;
using Common.Communication;
using Common.ServiceInterfaces.UIAdapter.SummaryJobs;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClientUI.ViewModels.CommandingWindow.DERGroup.DER
{
    public enum GeneratorType
    {
        Wind,
        Solar
    }

    public abstract class DERInformationViewModel : BaseDERCommandingEntityViewModel
    {
        private string name;
        private string imageUrl;
        private float activePower;
        private float nominalPower;

        public DERInformationViewModel(long derGlobalId, WCFClient<IDERGroupSummaryJob> derGroupSummary, string imageUrl = "") : base(derGlobalId, derGroupSummary)
        {
            this.imageUrl = imageUrl;

            LocateOnSchemaCommand = new RelayCommand(PublisLocateNodeEvent);
        }

        public string ImageSource
        {
            get { return imageUrl; }
            set
            {
                if (ImageSource != value)
                {
                    SetProperty(ref imageUrl, value);
                }
            }
        }

        public string Name
        {
            get { return name; }
            protected set
            {
                if (name != value)
                {
                    SetProperty(ref name, value);
                }
            }
        }

        public float NominalPower
        {
            get { return nominalPower; }
            set
            {
                if (nominalPower != value)
                {
                    SetProperty(ref nominalPower, value);
                }
            }
        }

        public float ActivePower
        {
            get { return activePower; }
            set
            {
                if (activePower != value)
                {
                    SetProperty(ref activePower, value);
                }
            }
        }

        public ICommand LocateOnSchemaCommand { get; set; }

        private void PublisLocateNodeEvent(object param)
        {
            SummaryManager.Instance.EventAggregator.GetEvent<SchemaNodeLocateEvent>().Publish(new SchemaNodeLocateEventArgs() { LocatingEntityGid = entityGid });
        }
    }
}
