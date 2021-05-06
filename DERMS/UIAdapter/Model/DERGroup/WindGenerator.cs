using Common.UIDataTransferObject;
using Common.UIDataTransferObject.DERGroup;

namespace UIAdapter.Model.DERGroup
{
    public class WindGenerator : Generator
    {
        public WindGenerator(long globalId) : base(globalId)
        {
        }

        public float StartUpSpeed { get; set; }

        public float CutOutSpeed { get; set; }

        public float NominalSpeed { get; set; }

        protected override void PopulateDTO(DistributedEnergyResourceDTO dto)
        {
            base.PopulateDTO(dto);
            DERGroupGeneratorSummaryDTO windGeneratorDTO = dto as DERGroupGeneratorSummaryDTO;
            if (windGeneratorDTO == null)
            {
                return;
            }

            windGeneratorDTO.CutOutSpeed = CutOutSpeed;
            windGeneratorDTO.NominalSpeed = NominalSpeed;
            windGeneratorDTO.StartUpSpeed = StartUpSpeed;
        }
    }
}
