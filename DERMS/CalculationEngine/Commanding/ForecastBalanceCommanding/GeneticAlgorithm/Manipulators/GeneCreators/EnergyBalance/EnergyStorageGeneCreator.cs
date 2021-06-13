using System;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Helpers;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.EnergyBalance.Genes;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.GeneCreators.EnergyBalance
{
    public class EnergyStorageGeneCreator : EnergyBalanceGeneCreator<EnergyStorageGene>
    {
        private EnergyStorageActivePowerCalculation energyStorageActivePowerCalculation;

        public EnergyStorageGeneCreator(EnergyStorageActivePowerCalculation energyStorageActivePowerCalculation)
        {
            this.energyStorageActivePowerCalculation = energyStorageActivePowerCalculation;
        }

        public override void Crossover(EnergyStorageGene child, EnergyStorageGene firstParent, EnergyStorageGene secondParent)
        {
            double coefficient = random.NextDouble();
            float considerActivePower = Convert.ToSingle(coefficient * firstParent.ActivePower + (1 - coefficient) * secondParent.ActivePower);

            float storageMinimalActivePower = energyStorageActivePowerCalculation.GetMinimumActivePower(child.Capacity, child.StateOfCharge, child.NominalPower);
            float storageMaximalActivePower = energyStorageActivePowerCalculation.GetMaximumActivePower(child.Capacity, child.StateOfCharge, child.NominalPower);

            float newActivePower = considerActivePower;

            if (considerActivePower < storageMinimalActivePower)
            {
                newActivePower = storageMinimalActivePower;
            }
            else if (considerActivePower > storageMaximalActivePower)
            {
                newActivePower = storageMaximalActivePower;
            }

            child.ActivePower = newActivePower;
        }
    }
}
