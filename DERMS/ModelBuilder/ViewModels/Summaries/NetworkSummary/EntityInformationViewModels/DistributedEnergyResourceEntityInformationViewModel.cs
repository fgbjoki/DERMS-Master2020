using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.UIDataTransferObject.NetworkModel;
using Common.UIDataTransferObject.NetworkModel.ConductingEquipment;

namespace ClientUI.ViewModels.Summaries.NetworkSummary.EntityInformationViewModels
{
    public abstract class DistributedEnergyResourceEntityInformationViewModel : ConductingEquipmentEntityInformationViewModel
    {
        private float nominalPower;

        public override void PopulateFields(NetworkModelEntityDTO entity)
        {
            base.PopulateFields(entity);

            DistributedEnergyResourceDTO dto = entity as DistributedEnergyResourceDTO;
            NominalPower = dto.NominalActivePower;
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
    }
}
