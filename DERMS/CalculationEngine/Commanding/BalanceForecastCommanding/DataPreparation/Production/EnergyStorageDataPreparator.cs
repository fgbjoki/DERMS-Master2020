using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.InitialPopulationCreation.Model;
using CalculationEngine.Model.DERCommanding;
using Common.ComponentStorage;
using System.Collections.Generic;
using System.Linq;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.DataPreparation.Production
{
    public class EnergyStorageDataPreparator
    {
        private IStorage<DistributedEnergyResource> ders;

        public EnergyStorageDataPreparator(IStorage<DistributedEnergyResource> ders)
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
