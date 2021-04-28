using Common.UIDataTransferObject.Schema;

namespace UIAdapter.Model.Schema
{
    public class SchemaEnergyBalance
    {
        public long EnergySourceGid { get; set; }
        public float ImportedEnergy { get; set; }
        public float DemandEnergy { get; set; }
        public float ProducedEnergy { get; set; }

        public static implicit operator SchemaEnergyBalanceDTO(SchemaEnergyBalance energyBalance)
        {
            if (energyBalance == null)
            {
                return null;
            }

            return new SchemaEnergyBalanceDTO()
            {
                ProducedEnergy = energyBalance.ProducedEnergy,
                ImportedEnergy = energyBalance.ImportedEnergy,
                DemandEnergy = energyBalance.DemandEnergy,
                EnergySourceGid = energyBalance.EnergySourceGid
            };
        }
    }
}
