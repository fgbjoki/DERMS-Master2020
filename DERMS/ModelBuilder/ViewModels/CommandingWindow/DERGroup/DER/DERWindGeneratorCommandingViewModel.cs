using Common.AbstractModel;
using Common.Communication;
using Common.ServiceInterfaces.UIAdapter.SummaryJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.UIDataTransferObject.DERGroup;

namespace ClientUI.ViewModels.CommandingWindow.DERGroup.DER
{
    public class DERWindGeneratorCommandingViewModel : BaseDERGeneratorCommandingViewModel
    {
        private float cutOutSpeed;
        private float startUpSpeed;
        private float nominalSpeed;

        public DERWindGeneratorCommandingViewModel(long derGlobalId, WCFClient<IDERGroupSummaryJob> derGroupSummary) : base(derGlobalId, derGroupSummary, "../../Resources/DER/windturbine1.png")
        {
        }

        public float StartUpSpeed
        {
            get { return startUpSpeed; }
            private set
            {
                if (startUpSpeed != value)
                {
                    SetProperty(ref startUpSpeed, value);
                }
            }
        }

        public float NominalSpeed
        {
            get { return nominalSpeed; }
            private set
            {
                if (nominalSpeed != value)
                {
                    SetProperty(ref nominalSpeed, value);
                }
            }
        }


        public float CutOutSpeed
        {
            get { return cutOutSpeed; }
            private set
            {
                if (cutOutSpeed != value)
                {
                    SetProperty(ref cutOutSpeed, value);
                }
            }
        }

        protected override void PopulateObject(DERGroupSummaryDTO dto)
        {
            base.PopulateObject(dto);

            DERGroupGeneratorSummaryDTO generator = dto.Generator;
            StartUpSpeed = generator.StartUpSpeed;
            NominalSpeed = generator.NominalSpeed;
            CutOutSpeed = generator.CutOutSpeed;
        }
    }
}
