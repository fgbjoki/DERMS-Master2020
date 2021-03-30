using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.UIDataTransferObject;
using Common.UIDataTransferObject.RemotePoints;

namespace ClientUI.Models.RemotePoints
{
    public class AnalogRemotePointSummaryItem : RemotePointSummaryItem
    {
        private float value;

        public float Value
        {
            get { return value; }
            set { SetProperty(ref this.value, value); }
        }

        protected override void UpdateProperties(IdentifiedObjectDTO entity)
        {
            base.UpdateProperties(entity);

            AnalogRemotePointSummaryDTO item = entity as AnalogRemotePointSummaryDTO;
            Value = item.Value;
        }

        protected override void UpdateProperties(IdentifiedObject entity)
        {
            base.UpdateProperties(entity);

            AnalogRemotePointSummaryItem item = entity as AnalogRemotePointSummaryItem;
            Value = item.Value;
        }
    }
}
