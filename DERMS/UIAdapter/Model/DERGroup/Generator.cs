using Common.AbstractModel;
using Common.UIDataTransferObject;
using Common.UIDataTransferObject.DERGroup;

namespace UIAdapter.Model.DERGroup
{
    public class Generator : DistributedEnergyResource<DERGroupGeneratorSummaryDTO>
    {
        public Generator(long globalId) : base(globalId)
        {
            DMSType dmsType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(globalId);

            if (dmsType == DMSType.WINDGENERATOR)
            {
                GeneratorType = GeneratorType.WIND;
            }
            else if (dmsType == DMSType.SOLARGENERATOR)
            {
                GeneratorType = GeneratorType.SOLAR;
            }
        }

        public GeneratorType GeneratorType { get; set; }

        public long EnergyStorageGid { get; set; }

        public override DERGroupGeneratorSummaryDTO CreateDTO()
        {
            DERGroupGeneratorSummaryDTO dto = new DERGroupGeneratorSummaryDTO();
            PopulateDTO(dto);

            return dto;
        }

        protected override void PopulateDTO(DistributedEnergyResourceDTO dto)
        {
            base.PopulateDTO(dto);

            DERGroupGeneratorSummaryDTO generatorDto = dto as DERGroupGeneratorSummaryDTO;
            if (generatorDto == null)
            {
                return;
            }

            generatorDto.GeneratorType = GeneratorType;
        }
    }
}
