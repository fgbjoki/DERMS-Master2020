using Common.UIDataTransferObject;
using Common.UIDataTransferObject.DERGroup;

namespace UIAdapter.Model.DERGroup
{
    public class DERGroup : DistributedEnergyResource<DERGroupSummaryDTO>
    {
        public DERGroup(long globalId) : base(globalId)
        {
        }

        public EnergyStorage EnergyStorage { get; set; }

        public Generator Generator { get; set; }

        public override float ActivePower
        {
            get
            {
                float totalActivePower = EnergyStorage.ActivePower;

                totalActivePower += Generator == null ? 0 : Generator.ActivePower;

                return totalActivePower;
            }
        }

        public override DERGroupSummaryDTO CreateDTO()
        {
            var dto = new DERGroupSummaryDTO();
            PopulateDTO(dto);

            return dto;
        }

        protected override void PopulateDTO(DistributedEnergyResourceDTO dto)
        {
            base.PopulateDTO(dto);

            DERGroupSummaryDTO derGroupDto = dto as DERGroupSummaryDTO;
            if (derGroupDto == null)
            {
                return;
            }

            derGroupDto.EnergyStorage = EnergyStorage.CreateDTO();
            derGroupDto.Generator = Generator.CreateDTO();
        }
    }
}
