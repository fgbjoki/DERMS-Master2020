using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.UIDataTransferObject;
using Common.UIDataTransferObject.DERGroup;

namespace ClientUI.Models.DERGroup
{
    public class DERGroupSummaryItem : IdentifiedObject
    {
        private float activePower;
        private float minimumNominalPower;
        private float maximumNominalPower;

        private string location;

        public DERGroupSummaryItem()
        {
            EnergyStorage = new EnergyStorage();
            Generator = new Generator();
        }

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

        public EnergyStorage EnergyStorage { get; set; }

        public Generator Generator { get; set; }

        public long LocationGid { get; set; }

        public float MinimumNominalPower
        {
            get { return minimumNominalPower; }
            set
            {
                if (minimumNominalPower != value)
                {
                    SetProperty(ref minimumNominalPower, value);
                }
            }
        }

        public float MaximumNominalPower
        {
            get { return maximumNominalPower; }
            set
            {
                if (maximumNominalPower != value)
                {
                    SetProperty(ref maximumNominalPower, value);
                }
            }
        }

        protected override void UpdateProperties(IdentifiedObjectDTO entity)
        {
            base.UpdateProperties(entity);

            DERGroupSummaryDTO dto = entity as DERGroupSummaryDTO;
            if (dto == null)
            {
                return;
            }

            LocationGid = dto.LocationGid;
            ActivePower = dto.ActivePower;
            EnergyStorage.Update(dto.EnergyStorage);
            Generator.Update(dto.Generator);

            MinimumNominalPower = -dto.EnergyStorage.NominalPower;
            MaximumNominalPower = dto.Generator.NominalPower + dto.EnergyStorage.NominalPower;
        }

        protected override void UpdateProperties(IdentifiedObject entity)
        {
            base.UpdateProperties(entity);

            DERGroupSummaryItem derGroupItem = entity as DERGroupSummaryItem;
            if (derGroupItem == null)
            {
                return;
            }

            ActivePower = derGroupItem.ActivePower;
            Generator.Update(derGroupItem.Generator);
            EnergyStorage.Update(derGroupItem.EnergyStorage);
        }
    }
}
