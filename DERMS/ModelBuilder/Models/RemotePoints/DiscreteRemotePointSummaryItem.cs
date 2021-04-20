using Common.UIDataTransferObject;
using Common.UIDataTransferObject.RemotePoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.Models.RemotePoints
{
    public class DiscreteRemotePointSummaryItem : RemotePointSummaryItem
    {
        private int value;
        private int normalValue;
        private int dom;
        private DiscreteAlarming alarm;

        public int Value
        {
            get { return value; }
            set { SetProperty(ref this.value, value); }
        }

        public int NormalValue
        {
            get { return normalValue; }
            set { SetProperty(ref normalValue, value); }
        }

        public int DOM
        {
            get { return dom; }
            set { SetProperty(ref dom, value); }
        }

        public DiscreteAlarming Alarm
        {
            get { return alarm; }
            set { SetProperty(ref alarm, value); }
        }

        protected override void UpdateProperties(IdentifiedObjectDTO entity)
        {
            base.UpdateProperties(entity);

            DiscreteRemotePointSummaryDTO item = entity as DiscreteRemotePointSummaryDTO;
            Value = item.Value;
            NormalValue = item.NormalValue;
            DOM = item.DOMManipulation;
            Alarm = item.Alarm;
        }

        protected override void UpdateProperties(IdentifiedObject entity)
        {
            base.UpdateProperties(entity);

            DiscreteRemotePointSummaryItem item = entity as DiscreteRemotePointSummaryItem;
            Value = item.Value;
            NormalValue = item.NormalValue;
            DOM = item.DOM;
            Alarm = item.Alarm;
        }
    }
}
