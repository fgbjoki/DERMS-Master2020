using Common.AbstractModel;
using Common.Communication;
using Common.ServiceInterfaces.UIAdapter.SummaryJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.UIDataTransferObject.DERGroup;

namespace ClientUI.ViewModels.CommandingWindow.DERGroup.DER
{
    public abstract class BaseDERGeneratorCommandingViewModel : DERInformationViewModel
    {
        public BaseDERGeneratorCommandingViewModel(long derGlobalId, WCFClient<IDERGroupSummaryJob> derGroupSummary, string imageUrl) : base(derGlobalId, derGroupSummary, imageUrl)
        {
            InitializeGeneratorType(derGlobalId);
        }

        public GeneratorType GeneratorType { get; private set; }

        protected override void PopulateObject(DERGroupSummaryDTO dto)
        {
            DERGroupGeneratorSummaryDTO generatorDTO = dto.Generator;
            ActivePower = generatorDTO.ActivePower;
            Name = generatorDTO.Name;
            NominalPower = generatorDTO.NominalPower;
        }

        private void InitializeGeneratorType(long derGlobalId)
        {
            DMSType dmsType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(derGlobalId);

            if (dmsType == DMSType.WINDGENERATOR)
            {
                GeneratorType = GeneratorType.Wind;
            }
            else if (dmsType == DMSType.SOLARGENERATOR)
            {
                GeneratorType = GeneratorType.Solar;
            }
        }
    }
}
