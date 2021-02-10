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
        public float Value { get; set; }

        protected override void UpdateProperties(IdentifiedObjectDTO entity)
        {
            base.UpdateProperties(entity);

            AnalogRemotePointSummaryDTO item = entity as AnalogRemotePointSummaryDTO;
            Value = item.Value;
        }
    }
}
