using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Helpers;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.InitialPopulationCreation.Model;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model.Genes;
using CalculationEngine.Extensions;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.InitialPopulationCreation.GeneCreators
{
    public class EnergyStorageInitialGeneCreator : InitialDERGeneCreator<EnergyStorageGene, EnergyStorageEntity>
    {
        private EnergyStorageActivePowerCalculator energyActivePowerCalculator;

        public EnergyStorageInitialGeneCreator(DomainParameters domainParameters)
        {
            energyActivePowerCalculator = new EnergyStorageActivePowerCalculator()
            {
                SimulationInterval = domainParameters.SimulationInterval
            };
        }

        protected override EnergyStorageGene CreateNewGene()
        {
            return new EnergyStorageGene();
        }

        protected override void PopulateFields(EnergyStorageGene gene, EnergyStorageEntity entity)
        {
            base.PopulateFields(gene, entity);
            gene.Capacity = entity.Capacity;
            gene.ActivePower = GetRandomActivePower(entity);
        }

        private float GetRandomActivePower(EnergyStorageEntity entity)
        {
            float minimumActivePower = energyActivePowerCalculator.GetMinimumActivePower(entity.Capacity, entity.StateOfCharge, entity.NominalPower);
            float maximumActivePower = energyActivePowerCalculator.GetMaximumActivePower(entity.Capacity, entity.StateOfCharge, entity.NominalPower);

            return random.Next(minimumActivePower, maximumActivePower);
        }
    }
}
