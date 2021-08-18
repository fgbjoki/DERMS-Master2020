using Common.ServiceInterfaces.CalculationEngine.DEROptimalCommanding;
using System.Collections.Generic;
using CalculationEngine.Model.DERCommanding;
using CalculationEngine.Model.DERStates;
using Common.ComponentStorage;

namespace CalculationEngine.Commanding.DEROptimalCommanding.CommandingProcessors
{
    public class NominalPowerPercentageOptimalCommandingProcessor : BaseDEROptimalCommandingProcessor<NominalPowerDEROptimalCommand>
    {
        public NominalPowerPercentageOptimalCommandingProcessor(IStorage<DERState> derStateStorage, IStorage<DistributedEnergyResource> derStorage) : base(derStateStorage, derStorage)
        {
        }

        protected override List<SuggestedDERValues> CreateCommandSequence(IEnumerable<EnergyStorage> energyStorages, NominalPowerDEROptimalCommand command)
        {
            List<SuggestedDERValues> suggestedDERValues = new List<SuggestedDERValues>();

            float totalNominalPower = CalculateTotalNominalPower(energyStorages);
            float coefficient = command.SetPoint / totalNominalPower;

            foreach (var energyStorage in energyStorages)
            {
                suggestedDERValues.Add(new SuggestedDERValues()
                {
                    GlobalId = energyStorage.GlobalId,
                    ActivePower = energyStorage.NominalPower * coefficient + energyStorage.ActivePower
                });
            }

            return suggestedDERValues;
        }

        private float CalculateTotalNominalPower(IEnumerable<EnergyStorage> energyStorages)
        {
            float total = 0;
            foreach (var energyStorage in energyStorages)
            {
                total += energyStorage.NominalPower;
            }

            return total;
        }
    }
}
