using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.UIDataTransferObject.NetworkModel;
using Common.UIDataTransferObject.NetworkModel.ConductingEquipment;

namespace ClientUI.ViewModels.Summaries.NetworkSummary.EntityInformationViewModels
{
    public class BreakerEntityInformationViewModel : ConductingEquipmentEntityInformationViewModel
    {
        private bool normalOpen;

        public BreakerEntityInformationViewModel()
        {
            MeasurementViewModels.Add(new DiscreteMeasurementEntityInformationViewModel());
        }

        public override void PopulateFields(NetworkModelEntityDTO entity)
        {
            base.PopulateFields(entity);

            BreakerDTO dto = entity as BreakerDTO;
            NormalOpen = dto.NormalOpen;
        }

        public bool NormalOpen
        {
            get { return normalOpen; }
            set
            {
                if (normalOpen != value)
                {
                    SetProperty(ref normalOpen, value);
                }
            }
        }
    }
}
