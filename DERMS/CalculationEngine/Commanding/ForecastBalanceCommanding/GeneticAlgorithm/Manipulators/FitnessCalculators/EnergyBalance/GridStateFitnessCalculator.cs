using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.EnergyBalance.Genes;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.Chromosomes;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.FitnessCalculators.EnergyBalance.GeneFitnessCalculators;
using Common.AbstractModel;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Helpers.FitnessParameters;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.FitnessCalculators.EnergyBalance
{
    public class GridStateFitnessCalculator : BaseFitnessCalculator<GridStateGene, BoundaryParameteres>
    {
        private EnergyStorageGeneFitnessCalculator energyStorageFitnessCalculator;
        private GeneratorGeneFitnessCalculator generatorFitnessCalculator;
        private NetworkEnergyImportFitnessCalculator networkImportFitnessCalculator;

        private BoundaryParameteres fitnessParameters;

        public GridStateFitnessCalculator(BoundaryParameteres fitnessParameters)
        {
            this.fitnessParameters = fitnessParameters;

            energyStorageFitnessCalculator = new EnergyStorageGeneFitnessCalculator();
            generatorFitnessCalculator = new GeneratorGeneFitnessCalculator();
            networkImportFitnessCalculator = new NetworkEnergyImportFitnessCalculator();
        }

        public override void Calculate(Chromosome<GridStateGene> chromosome)
        {
            LoadFitnessParameters();

            double fitnessValue = 0;
            float response = 0;

            foreach (var gridStateGene in chromosome.Genes)
            {
                float geneResponse = 0;
                foreach (var gene in gridStateGene.DERGenes)
                {
                    IDERGeneFitnessCalculator geneFitnessCalculator = null;

                    if (gene.DMSType == DMSType.ENERGYSTORAGE)
                    {
                        geneFitnessCalculator = energyStorageFitnessCalculator;
                    }
                    else if (gene.DMSType == DMSType.SOLARGENERATOR || gene.DMSType == DMSType.WINDGENERATOR)
                    {
                        geneFitnessCalculator = generatorFitnessCalculator;
                    }

                    fitnessValue += geneFitnessCalculator.Calculate(gene);
                    geneResponse += gene.ActivePower;
                }

                double networkFitnessValue = networkImportFitnessCalculator.Calculate(gridStateGene.EnergyDemand, geneResponse);
                if (double.IsInfinity(networkFitnessValue))
                {
                    chromosome.FitnessValue = networkFitnessValue;
                    return;
                }

                response += geneResponse;
                fitnessValue += networkFitnessValue;
            }

            chromosome.FitnessValue = fitnessValue;
        }

        private float CostOfNetworkEnergyUse
        {
            set
            {
                networkImportFitnessCalculator.KWHCost = value;
            }
        }
        private float CostOfEnergyStorageUse
        {
            set
            {
                energyStorageFitnessCalculator.CostOfEnergyUse = value;
            }
        }
        private float CostOfGeneratorShutdown
        {
            set
            {
                generatorFitnessCalculator.CostOfShutdownPerKWH = value;
            }
        }

        private ulong SimulationInterval
        {
            set
            {
                networkImportFitnessCalculator.SimulationInterval = generatorFitnessCalculator.IntervalSimulation = energyStorageFitnessCalculator.IntervalSimulation = value;
            }
        }

        private void LoadFitnessParameters()
        {
            SimulationInterval = fitnessParameters.SimulationInterval;
            CostOfGeneratorShutdown = fitnessParameters.CostOfGeneratorShutDownPerKWH;
            CostOfEnergyStorageUse = fitnessParameters.CostOfEnergyStorageUsePerKWH;
            CostOfNetworkEnergyUse = fitnessParameters.CostOfNetworkEnergyImportPerKWH;
        }
    }
}
