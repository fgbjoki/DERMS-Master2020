using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.InitialPopulationCreators.Model;
using CalculationEngine.Model.DERCommanding;
using Common.ComponentStorage;
using System.Collections.Generic;
using System.Linq;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.ModelAggeragation
{
    public class EnergyStorageAggregator
    {
        private IStorage<DistributedEnergyResource> ders;

        public EnergyStorageAggregator(IStorage<DistributedEnergyResource> ders)
        {
            this.ders = ders;
        }

        public List<EnergyStorageEntity> CreateEntities()
        {
            List<EnergyStorageEntity> energyStorages = new List<EnergyStorageEntity>();
            foreach (var energyStorage in ders.GetAllEntities().Where(x => x.DMSType == Common.AbstractModel.DMSType.ENERGYSTORAGE).Cast<EnergyStorage>())
            {
                var energyStorageEntity = new EnergyStorageEntity()
                {
                    GlobalId = energyStorage.GlobalId,
                    NominalPower = energyStorage.NominalPower,
                    Capacity = energyStorage.Capacity,
                };

                energyStorages.Add(energyStorageEntity);
            }

            return energyStorages;
        }
    }
}
