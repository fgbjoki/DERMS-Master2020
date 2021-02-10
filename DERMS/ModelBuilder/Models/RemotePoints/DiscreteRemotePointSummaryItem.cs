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
        public int Value { get; set; }

        public int NormalValue { get; set; }

        protected override void UpdateProperties(IdentifiedObjectDTO entity)
        {
            base.UpdateProperties(entity);

            DiscreteRemotePointSummaryDTO item = entity as DiscreteRemotePointSummaryDTO;
            Value = item.Value;
            NormalValue = item.NormalValue;
        }
    }
}
