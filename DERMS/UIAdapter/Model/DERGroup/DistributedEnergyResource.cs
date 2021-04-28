using Common.UIDataTransferObject;
using UIAdapter.SummaryJobs;

namespace UIAdapter.Model.DERGroup
{
    public abstract class DistributedEnergyResource<T> : SummaryItem<T>
        where T : DistributedEnergyResourceDTO
    {
        public DistributedEnergyResource(long globalId) : base(globalId)
        {
        }

        public virtual float ActivePower { get; set; }
        public long ActivePowerMeasurementGid { get; set; }

        public float NominalPower { get; set; }

        protected virtual void PopulateDTO(DistributedEnergyResourceDTO dto)
        {
            dto.ActivePower = ActivePower;
            dto.NominalPower = NominalPower;
            dto.Name = Name;
            dto.GlobalId = GlobalId;
        }
    }
}
