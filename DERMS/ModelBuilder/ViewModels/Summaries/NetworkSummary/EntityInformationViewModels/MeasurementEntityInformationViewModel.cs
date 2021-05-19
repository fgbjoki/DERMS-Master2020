using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.UIDataTransferObject.NetworkModel;
using Common.AbstractModel;

namespace ClientUI.ViewModels.Summaries.NetworkSummary.EntityInformationViewModels
{
    public abstract class MeasurementEntityInformationViewModel : BaseNetworkModelEntityInformationViewModel
    {
        private MeasurementType measurementType;
        private SignalDirection signalDirection;
        private int address;

        public MeasurementType MeasurementType
        {
            get { return measurementType; }
            set
            {
                if (measurementType != value)
                {
                    SetProperty(ref measurementType, value);
                }
            }
        }

        public SignalDirection SignalDirection
        {
            get { return signalDirection; }

            set
            {
                if (signalDirection != value)
                {
                    SetProperty(ref signalDirection, value);
                }
            }
        }

        public int Address
        {
            get { return address; }

            set
            {
                if (address != value)
                {
                    SetProperty(ref address, value);
                }
            }
        }

        public override void PopulateFields(NetworkModelEntityDTO entity)
        {
            base.PopulateFields(entity);
            MeasurementDTO dto = entity as MeasurementDTO;

            Address = dto.Address;
            SignalDirection = dto.SignalDirection;
            MeasurementType = dto.MeasurementType;
        }
    }
}
