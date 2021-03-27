using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.UIDataTransferObject;
using Common.UIDataTransferObject.Schema;

namespace ClientUI.Models.Schema
{
    public class EnergySourceBrowseSchemaItem : IdentifiedObject
    {
        private string substationName;

        public EnergySourceBrowseSchemaItem()
        {
        }

        public string SubstationName
        {
            get { return substationName; }
            set { SetProperty(ref substationName, value); }
        }

        protected override void UpdateProperties(IdentifiedObjectDTO entity)
        {
            base.UpdateProperties(entity);

            EnergySourceDTO dto = entity as EnergySourceDTO;

            if (dto == null)
            {
                return;
            }

            SubstationName = dto.SubstationName;
        }
    }
}
