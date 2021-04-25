using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.UIDataTransferObject;
using Common.UIDataTransferObject.DERGroup;

namespace ClientUI.Models.DERGroup
{
    public class EnergyStorage : IdentifiedObject
    {
        private float activePower;
        private float stateOfCharge;
        private float nominalPower;

        public float StateOfCharge
        {
            get { return stateOfCharge; }
            set
            {
                if (stateOfCharge != value)
                {
                    SetProperty(ref stateOfCharge, value);
                }
            }
        }

        public float NominalPower
        {
            get { return nominalPower; }
            set
            {
                if (nominalPower != value)
                {
                    SetProperty(ref nominalPower, value);
                }
            }
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

        protected override void UpdateProperties(IdentifiedObjectDTO entity)
        {
            base.UpdateProperties(entity);

            DERGroupEnergyStorageSummaryDTO dto = entity as DERGroupEnergyStorageSummaryDTO;
            if (dto == null)
            {
                return;
            }

            ActivePower = dto.ActivePower;
            StateOfCharge = dto.StateOfCharge * 100;
            NominalPower = dto.NominalPower;
        }

        protected override void UpdateProperties(IdentifiedObject entity)
        {
            base.UpdateProperties(entity);

            EnergyStorage energyStorage = entity as EnergyStorage;
            if (energyStorage == null)
            {
                return;
            }

            ActivePower = energyStorage.ActivePower;
            StateOfCharge = energyStorage.StateOfCharge;
            NominalPower = energyStorage.NominalPower;
        }
    }
}
