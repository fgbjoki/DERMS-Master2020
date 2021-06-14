using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Helpers;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Helpers.FitnessParameters;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.InitialPopulationCreators.Model;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.EnergyBalance.Genes;
using CalculationEngine.Extensions;
using System.Collections.Generic;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.InitialPopulationCreators.GeneCreators
{
    public class EnergyStorageInitialGeneCreator : InitialDERGeneCreator<EnergyStorageGene, EnergyStorageEntity>
    {
        private EnergyStorageActivePowerCalculation energyActivePowerCalculator;

        public EnergyStorageInitialGeneCreator(BoundaryParameteres boundaryParameters)
        {
            energyActivePowerCalculator = new EnergyStorageActivePowerCalculation(boundaryParameters);
            LowerBoundStateOfCharge = boundaryParameters.LowerBoundStateOfCharge;
            UpperBoundStateOfCharge = boundaryParameters.UpperBoundStateOfCharge;
            SimulationInterval = boundaryParameters.SimulationInterval;
        }

        public float LowerBoundStateOfCharge { get; set; }
        public float UpperBoundStateOfCharge { get; set; }
        public ulong SimulationInterval { get; set; }

        protected override EnergyStorageGene CreateNewGene(EnergyStorageEntity entity)
        {
            return new EnergyStorageGene(entity.GlobalId, entity.NominalPower, entity.ActivePower);
        }

        public override List<EnergyStorageGene> CreateGene(EnergyStorageEntity entity, int numberOfGenesToCreate)
        {
            List<EnergyStorageGene> energyStorages = new List<EnergyStorageGene>(numberOfGenesToCreate);

            EnergyStorageGene previousGene = CreateFirstGene(entity);
            previousGene.StateOfCharge = random.Next(LowerBoundStateOfCharge, UpperBoundStateOfCharge);
            energyStorages.Add(previousGene);

            for (int i = 1; i < numberOfGenesToCreate; i++)
            {
                EnergyStorageGene energyStorageGene = new EnergyStorageGene(entity.GlobalId, entity.NominalPower, entity.ActivePower)
                {
                    StateOfCharge = CalculateStateOfCharge(previousGene)
                };

                PopulateFields(energyStorageGene, entity);
                energyStorages.Add(energyStorageGene);
                previousGene = energyStorageGene;
            }

            return energyStorages;
        }

        private EnergyStorageGene CreateFirstGene(EnergyStorageEntity entity)
        {
            var gene = new EnergyStorageGene(entity.GlobalId, entity.NominalPower, entity.ActivePower);
            PopulateFields(gene, entity);

            return gene;
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

        private float CalculateStateOfCharge(EnergyStorageGene energyStorageGene)
        {
            float maxCapacity = energyStorageGene.Capacity * 3600;
            float currentCapacityAP = maxCapacity * energyStorageGene.StateOfCharge;
            float usedEnergy = energyStorageGene.ActivePower * SimulationInterval;
            float newCapacityAP = currentCapacityAP - usedEnergy;
            return newCapacityAP / maxCapacity;
        }
    }
}
