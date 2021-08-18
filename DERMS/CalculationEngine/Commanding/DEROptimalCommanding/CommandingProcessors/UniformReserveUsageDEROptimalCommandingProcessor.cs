using Common.ServiceInterfaces.CalculationEngine.DEROptimalCommanding;
using System.Collections.Generic;
using CalculationEngine.Model.DERCommanding;
using CalculationEngine.Model.DERStates;
using Common.ComponentStorage;
using System.Linq;

namespace CalculationEngine.Commanding.DEROptimalCommanding.CommandingProcessors
{
    internal class ReserveCalculation
    {
        public ReserveCalculation(float reserveActivePower, EnergyStorage energyStorage)
        {
            ReserveActivePower = reserveActivePower;
            EnergyStorage = energyStorage;
        }

        public float ReserveActivePower { get; set; }
        public EnergyStorage EnergyStorage { get; set; }
    }

    public class UniformReserveUsageDEROptimalCommandingProcessor : BaseDEROptimalCommandingProcessor<UniformReserveDEROptimalCommand>
    {
        public UniformReserveUsageDEROptimalCommandingProcessor(IStorage<DERState> derStateStorage, IStorage<DistributedEnergyResource> derStorage) : base(derStateStorage, derStorage)
        {
        }

        protected override List<SuggestedDERValues> CreateCommandSequence(IEnumerable<EnergyStorage> energyStorages, UniformReserveDEROptimalCommand command)
        {
            List<SuggestedDERValues> suggestedValues = new List<SuggestedDERValues>();
            List<ReserveCalculation> reserves = CreateReserveCalculation(energyStorages);

            float totalReserve = reserves.Sum(x => x.ReserveActivePower);
            float coefficient = command.SetPoint / totalReserve;

            foreach (var reserve in reserves)
            {
                suggestedValues.Add(new SuggestedDERValues()
                {
                    GlobalId = reserve.EnergyStorage.GlobalId,
                    ActivePower = reserve.ReserveActivePower * coefficient + reserve.EnergyStorage.ActivePower
                });
            }

            return suggestedValues;
        }

        private List<ReserveCalculation> CreateReserveCalculation(IEnumerable<EnergyStorage> energyStorages)
        {
            List<ReserveCalculation> reserves = new List<ReserveCalculation>();

            foreach (var energyStorage in energyStorages)
            {
                float reserve = energyStorage.NominalPower - energyStorage.ActivePower;
                reserves.Add(new ReserveCalculation(reserve, energyStorage));
            }

            return reserves;
        }
    }
}
