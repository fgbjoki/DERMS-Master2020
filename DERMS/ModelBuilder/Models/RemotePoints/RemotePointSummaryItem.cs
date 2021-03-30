using Common.UIDataTransferObject;
using Common.UIDataTransferObject.RemotePoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.Models.RemotePoints
{
    public abstract class RemotePointSummaryItem : IdentifiedObject
    {
        private int address;

        public int Address
        {
            get { return address; }
            set { SetProperty(ref address, value); }
        }

        protected override void UpdateProperties(IdentifiedObjectDTO entity)
        {
            base.UpdateProperties(entity);

            RemotePointSummaryDTO item = entity as RemotePointSummaryDTO;

            Address = item.Address;
        }
    }
}
