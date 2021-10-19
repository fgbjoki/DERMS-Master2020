using Common.UIDataTransferObject.NetworkModel;
using Common.UIDataTransferObject.NetworkModel.ConductingEquipment;
using DERMS;

namespace ClientUI.ViewModels.Summaries.NetworkSummary.EntityInformationViewModels
{
    public class EnergyConsumerEntityInformationViewModel : ConductingEquipmentEntityInformationViewModel
    {
        private float pfixed;
        private CustomConsumerType type;

        public EnergyConsumerEntityInformationViewModel()
        {
            MeasurementViewModels.Add(new AnalogMeasurementEntityInformationViewModel());
        }

        public override void PopulateFields(NetworkModelEntityDTO entity)
        {
            base.PopulateFields(entity);
            ConsumerDTO dto = entity as ConsumerDTO;

            Pfixed = dto.Pfixed;
            Type = dto.Type;
        }


        public float Pfixed
        {
            get { return pfixed; }
            set
            {
                if(pfixed != value)
                {
                    SetProperty(ref pfixed, value);
                }
            }
        }

        public CustomConsumerType Type
        {
            get { return type; }
            set
            {
                if (type != value)
                {
                    SetProperty(ref type, value);
                }
            }
        }
    }
}
