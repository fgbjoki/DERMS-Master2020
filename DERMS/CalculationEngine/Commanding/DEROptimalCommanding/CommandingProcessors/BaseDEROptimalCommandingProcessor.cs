using CalculationEngine.Model.DERCommanding;
using CalculationEngine.Model.DERStates;
using Common.ComponentStorage;
using Common.ServiceInterfaces.CalculationEngine.DEROptimalCommanding;
using System.Collections.Generic;
using System.Linq;

namespace CalculationEngine.Commanding.DEROptimalCommanding.CommandingProcessors
{
    public class SuggestedDERValues
    {
        public long GlobalId { get; set; }
        public float ActivePower { get; set; }
    }

    public abstract class BaseDEROptimalCommandingProcessor<T> : IDEROptimalCommandingProcessor
        where T : DEROptimalCommand
    {
        private IStorage<DERState> derStateStorage;
        private IStorage<DistributedEnergyResource> derStorage;

        protected BaseDEROptimalCommandingProcessor(IStorage<DERState> derStateStorage, IStorage<DistributedEnergyResource> derStorage)
        {
            this.derStateStorage = derStateStorage;
            this.derStorage = derStorage;
        }

        public List<SuggestedDERValues> CreateCommandSequence(DEROptimalCommand command)
        {
            return CreateCommandSequence(command as T);
        }

        protected abstract List<SuggestedDERValues> CreateCommandSequence(IEnumerable<EnergyStorage> energyStorages, T command);

        private List<SuggestedDERValues> CreateCommandSequence(T command)
        {
            ICollection<long> energyStorageGids = GetEnergizedEnergyStorages();
            if (energyStorageGids.Count == 0)
            {
                return new List<SuggestedDERValues>();
            }

            IEnumerable<EnergyStorage> energyStorages = derStorage.GetAllEntities().Where(x => x.DMSType == Common.AbstractModel.DMSType.ENERGYSTORAGE && energyStorageGids.Contains(x.GlobalId)).Cast<EnergyStorage>();

            return CreateCommandSequence(energyStorages, command);
        }

        private ICollection<long> GetEnergizedEnergyStorages()
        {
            List<DERState> energyStorages = derStateStorage.GetAllEntities();
            energyStorages.RemoveAll(x => x.DMSType != Common.AbstractModel.DMSType.ENERGYSTORAGE || x.IsEnergized == false);

            return energyStorages.Select(x => x.GlobalId).ToList();
        }
    }
}
