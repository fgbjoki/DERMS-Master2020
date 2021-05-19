using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.UIDataTransferObject.NetworkModel;

namespace ClientUI.ViewModels.Summaries.NetworkSummary.EntityInformationViewModels
{
    public class AnalogMeasurementEntityInformationViewModel : MeasurementEntityInformationViewModel
    {
        private float minValue;
        private float maxValue;

        public float MaxValue
        {
            get { return maxValue; }
            set
            {
                if (maxValue != value)
                {
                    SetProperty(ref maxValue, value);
                }
            }
        }

        public float MinValue
        {
            get { return minValue; }
            set
            {
                if (minValue != value)
                {
                    SetProperty(ref minValue, value);
                }
            }
        }

        public override void PopulateFields(NetworkModelEntityDTO entity)
        {
            base.PopulateFields(entity);

            AnalogMeasurementDTO dto = entity as AnalogMeasurementDTO;
            MinValue = dto.MinValue;
            MaxValue = dto.MaxValue;
        }
    }
}
