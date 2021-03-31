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
        private float maxValue;
        private float minValue;
        private AnalogAlarming alarm;

        public float Value
        {
            get { return value; }
            set { SetProperty(ref this.value, value); }
        }

        public float MaxValue
        {
            get { return maxValue; }
            set { SetProperty(ref maxValue, value); }
        }

        public float MinValue
        {
            get { return minValue; }
            set { SetProperty(ref minValue, value); }
        }

        public AnalogAlarming Alarm
        {
            get { return alarm; }
            set { SetProperty(ref alarm, value); }
        }

        protected override void UpdateProperties(IdentifiedObjectDTO entity)
        {
            base.UpdateProperties(entity);

            AnalogRemotePointSummaryDTO item = entity as AnalogRemotePointSummaryDTO;
            Value = item.Value;
            MaxValue = item.MaxValue;
            MinValue = item.MinValue;
            Alarm = item.Alarm;
        }

        protected override void UpdateProperties(IdentifiedObject entity)
        {
            base.UpdateProperties(entity);

            AnalogRemotePointSummaryItem item = entity as AnalogRemotePointSummaryItem;
            Value = item.Value;
            MaxValue = item.MaxValue;
            MinValue = item.MinValue;
            Alarm = item.Alarm;
        }
    }
}
