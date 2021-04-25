using Common.UIDataTransferObject;
using Common.UIDataTransferObject.DERGroup;

namespace UIAdapter.Model.DERGroup
{
    public class EnergyStorage : DistributedEnergyResource<DERGroupEnergyStorageSummaryDTO>
    {
        public EnergyStorage(long globalId) : base(globalId)
        {
        }

        public float StateOfCharge { get; set; }

        public long StateOfChargeMeasurementGid { get; set; }

        public override DERGroupEnergyStorageSummaryDTO CreateDTO()
        {
            DERGroupEnergyStorageSummaryDTO dto = new DERGroupEnergyStorageSummaryDTO();
            PopulateDTO(dto);

            return dto;
        }

        protected override void PopulateDTO(DistributedEnergyResourceDTO dto)
        {
            base.PopulateDTO(dto);

            DERGroupEnergyStorageSummaryDTO energyStorageDTO = dto as DERGroupEnergyStorageSummaryDTO;
            if (energyStorageDTO == null)
            {
                return;
            }

            energyStorageDTO.StateOfCharge = StateOfCharge;
        }
    }
}
