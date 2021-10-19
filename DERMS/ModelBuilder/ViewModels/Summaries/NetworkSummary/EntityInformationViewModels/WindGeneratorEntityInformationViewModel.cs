using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.UIDataTransferObject.NetworkModel;
using Common.UIDataTransferObject.NetworkModel.ConductingEquipment;

namespace ClientUI.ViewModels.Summaries.NetworkSummary.EntityInformationViewModels
{
    public class WindGeneratorEntityInformationViewModel : DistributedEnergyResourceEntityInformationViewModel
    {
        private float nominalActivePower;
        private float startUpSpeed;
        private float nominalSpeed;
        private float cutOutSpeed;

        public WindGeneratorEntityInformationViewModel()
        {
            MeasurementViewModels.Add(new AnalogMeasurementEntityInformationViewModel());
        }

        public override void PopulateFields(NetworkModelEntityDTO entity)
        {
            base.PopulateFields(entity);
            WindGeneratorDTO dto = entity as WindGeneratorDTO;

            NominalActivePower = dto.NominalActivePower;
            CutOutSpeed = dto.CutOutSpeed;
            NominalSpeed = dto.NominalSpeed;
            StartUpSpeed = dto.StartUpSpeed;
        }


        public float NominalActivePower
        {
            get { return nominalActivePower; }
            set
            {
                if (nominalActivePower != value)
                {
                    SetProperty(ref nominalActivePower, value);
                }
            }
        }

        public float StartUpSpeed
        {
            get { return startUpSpeed; }
            set
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
            set
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
            set
            {
                if (cutOutSpeed != value)
                {
                    SetProperty(ref cutOutSpeed, value);
                }
            }
        }

    }
}
