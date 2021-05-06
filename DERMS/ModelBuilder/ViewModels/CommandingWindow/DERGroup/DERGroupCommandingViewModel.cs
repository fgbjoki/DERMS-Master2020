using Common.Communication;
using Common.ServiceInterfaces.UIAdapter.SummaryJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.UIDataTransferObject.DERGroup;
using ClientUI.Models.DERGroup.CommandingWindow.DERGroup;
using Common.AbstractModel;

namespace ClientUI.ViewModels.CommandingWindow.DERGroup
{
    public class DERGroupCommandingViewModel : BaseDERCommandingEntityViewModel
    {
        private DERGids derGids;

        private Generator generator;
        private EnergyStorage energyStorage;

        private float derGroupActivePower;

        public DERGroupCommandingViewModel(DERGids derGids, WCFClient<IDERGroupSummaryJob> derGroupSummary) : base(derGids.EnergyStorageGid, derGroupSummary)
        {
            this.derGids = derGids;
            CreateEntities();
        }

        public Generator Generator
        {
            get { return generator; }
        }

        public EnergyStorage EnergyStorage
        {
            get { return energyStorage; }
        }

        public float DERGroupActivePower
        {
            get { return derGroupActivePower; }
            set
            {
                if (derGroupActivePower != value)
                {
                    SetProperty(ref derGroupActivePower, value);
                }
            }
        }

        protected override void PopulateObject(DERGroupSummaryDTO dto)
        {
            Generator?.Update(dto.Generator);
            EnergyStorage.Update(dto.EnergyStorage);
            DERGroupActivePower = dto.ActivePower;

            RaisePropertyChanged(DERGroupActivePower.ToString());
        }

        private void CreateEntities()
        {
            energyStorage = new EnergyStorage() { GlobalId = derGids.EnergyStorageGid };
            generator = CreateGenerator();
        }

        private Generator CreateGenerator()
        {
            if (derGids.GeneratorGid == 0)
            {
                return null;
            }

            DMSType dmsType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(derGids.GeneratorGid);
            if (dmsType == DMSType.SOLARGENERATOR)
            {
                return new SolarPanel() { GlobalId = derGids.GeneratorGid };
            }
            else if (dmsType == DMSType.WINDGENERATOR)
            {
                return new WindGenerator() { GlobalId = derGids.GeneratorGid };
            }

            return null;
        }
    }
}
