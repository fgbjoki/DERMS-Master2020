using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.UIDataTransferObject;
using Common.UIDataTransferObject.DERGroup;

namespace ClientUI.Models.DERGroup
{
    public class Generator : IdentifiedObject
    {
        private float activePower;

        public float ActivePower
        {
            get { return activePower; }
            set
            {
                if (activePower != value)
                {
                    SetProperty(ref activePower, value);
                }
            }
        }

        protected override void UpdateProperties(IdentifiedObjectDTO entity)
        {
            base.UpdateProperties(entity);

            DERGroupGeneratorSummaryDTO dto = entity as DERGroupGeneratorSummaryDTO;
            if (dto == null)
            {
                return;
            }

            ActivePower = dto.ActivePower;
        }

        protected override void UpdateProperties(IdentifiedObject entity)
        {
            base.UpdateProperties(entity);

            Generator generator = entity as Generator;
            if (generator == null)
            {
                return;
            }

            ActivePower = generator.ActivePower;
        }
    }
}
