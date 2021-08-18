using Common.UIDataTransferObject.Schema;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.Models.Schema
{
    public class SchemaEnergyBalance : BindableBase
    {
        private float importedEnergy;
        private float demandEnergy;
        private float producedEnergy;

        private float solarEnergyProduced;
        private float windEnergyProduced;
        private float energyStorageEnergyProduced;

        public long EnergySourceGid { get; set; }

        public float ImportedEnergy
        {
            get { return importedEnergy; }
            set
            {
                if (importedEnergy != value)
                {
                    SetProperty(ref importedEnergy, value);
                }
            }
        }

        public float DemandEnergy
        {
            get { return demandEnergy; }
            set
            {
                if (demandEnergy != value)
                {
                    SetProperty(ref demandEnergy, value);
                }
            }
        }

        public float ProducedEnergy
        {
            get { return producedEnergy; }
            set
            {
                if (producedEnergy != value)
                {
                    SetProperty(ref producedEnergy, value);
                }
            }
        }

        public float SolarEnergyProduced
        {
            get { return solarEnergyProduced; }
            set
            {
                if (solarEnergyProduced != value)
                {
                    SetProperty(ref solarEnergyProduced, value);
                }
            }
        }

        public float WindEnergyProduced
        {
            get { return windEnergyProduced; }
            set
            {
                if (windEnergyProduced != value)
                {
                    SetProperty(ref windEnergyProduced, value);
                }
            }
        }

        public float EnergyStorageEnergyProduced
        {
            get { return energyStorageEnergyProduced; }
            set
            {
                if (energyStorageEnergyProduced != value)
                {
                    SetProperty(ref energyStorageEnergyProduced, value);
                }
            }
        }

        public static implicit operator SchemaEnergyBalance(SchemaEnergyBalanceDTO energyBalance)
        {
            if (energyBalance == null)
            {
                return null;
            }

            return new SchemaEnergyBalance()
            {
                ProducedEnergy = energyBalance.ProducedEnergy,
                ImportedEnergy = energyBalance.ImportedEnergy,
                DemandEnergy = energyBalance.DemandEnergy,
                EnergySourceGid = energyBalance.EnergySourceGid,
                SolarEnergyProduced = energyBalance.SolarEnergyProduction,
                WindEnergyProduced = energyBalance.WindEnergyProduction,
                EnergyStorageEnergyProduced = energyBalance.EnergyStorageEnergyProduction
            };
        }
    }
}
