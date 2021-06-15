using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Helpers;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.EnergyBalance.Genes;
using Common.AbstractModel;
using System.Collections.Generic;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.GeneCreators.EnergyBalance
{
    public class GridStateGeneCreator : EnergyBalanceGeneCreator<GridStateGene>
    {
        private EnergyStorageGeneCreator energyStorageGeneCreator;
        private GeneratorGeneCreator generatorGeneCreator;

        public GridStateGeneCreator(EnergyStorageActivePowerCalculation esAPCalculator)
        {
            energyStorageGeneCreator = new EnergyStorageGeneCreator(esAPCalculator);
            generatorGeneCreator = new GeneratorGeneCreator();
        }

        public override void Crossover(GridStateGene child, GridStateGene firstParent, GridStateGene secondParent)
        {
            Dictionary<long, float> newStateOfCharge = new Dictionary<long, float>();

            for (int i = 0; i < child.DERGenes.Count; i++)
            {
                DERGene childGene = child.DERGenes[i];
                DERGene firstParentGene = firstParent.DERGenes[i];
                DERGene secondParentGene = secondParent.DERGenes[i];

                if (childGene.DMSType == DMSType.ENERGYSTORAGE)
                {
                    EnergyStorageGene childEnergyStorage = childGene as EnergyStorageGene;
                    EnergyStorageGene firstParentEnergyStorage = firstParentGene as EnergyStorageGene;
                    EnergyStorageGene secondParentEnergyStorage = secondParentGene as EnergyStorageGene;

                    if (i > 0)
                    {
                        childEnergyStorage.StateOfCharge = newStateOfCharge[childEnergyStorage.GlobalId];
                    }

                    energyStorageGeneCreator.Crossover(childEnergyStorage, firstParentEnergyStorage, secondParentEnergyStorage);

                    PopulateCurrentStateOfCharge(newStateOfCharge, childEnergyStorage);
                }
                else
                {
                    generatorGeneCreator.Crossover(childGene as GeneratorGene, firstParentGene as GeneratorGene, secondParentGene as GeneratorGene);
                }
            }
        }

        private void PopulateCurrentStateOfCharge(Dictionary<long ,float> stateOfCharge, EnergyStorageGene energyStorageGene)
        {
            float maxCapacity = energyStorageGene.Capacity * 3600;
            float currentCapacityAP = maxCapacity * energyStorageGene.StateOfCharge;
            float usedEnergy = energyStorageGene.ActivePower * SimulationInterval;
            float newCapacityAP = currentCapacityAP - usedEnergy;
            float newStateOfCharge = newCapacityAP / maxCapacity;

            stateOfCharge[energyStorageGene.GlobalId] = newStateOfCharge;
        }
    }
}
