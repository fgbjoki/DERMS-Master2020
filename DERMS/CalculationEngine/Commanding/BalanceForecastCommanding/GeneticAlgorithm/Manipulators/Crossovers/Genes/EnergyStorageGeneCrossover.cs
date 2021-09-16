using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Helpers;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model.Genes;
using System;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Manipulators.Crossovers.Genes
{
    public class EnergyStorageGeneCrossover : BaseGeneCrossover<EnergyStorageGene>, IDERGeneCrossover
    {
        private EnergyStorageActivePowerCalculator energyStoragePowerCalculator;

        public EnergyStorageGeneCrossover(EnergyStorageActivePowerCalculator energyStoragePowerCalculator)
        {
            this.energyStoragePowerCalculator = energyStoragePowerCalculator;
        }

        public DERGene Crossover(DERGene firstParent, DERGene secondParent)
        {
            return Crossover(firstParent as EnergyStorageGene, secondParent as EnergyStorageGene);
        }

        public override EnergyStorageGene Crossover(EnergyStorageGene firstParent, EnergyStorageGene secondParent)
        {
            EnergyStorageGene child = (EnergyStorageGene)firstParent.Clone();

            double coefficient = random.NextDouble();
            float considerActivePower = Convert.ToSingle(coefficient * firstParent.ActivePower + (1 - coefficient) * secondParent.ActivePower);

            float storageMinimalActivePower = energyStoragePowerCalculator.GetMinimumActivePower(child.Capacity, child.StateOfCharge, child.NominalPower);
            float storageMaximalActivePower = energyStoragePowerCalculator.GetMaximumActivePower(child.Capacity, child.StateOfCharge, child.NominalPower);

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

            return child;
        }
    }
}
