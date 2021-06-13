using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.EnergyBalance.Genes;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.Chromosomes;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.FitnessCalculators.EnergyBalance.GeneFitnessCalculators;
using Common.AbstractModel;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Helpers.FitnessParameters;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.FitnessCalculators.EnergyBalance
{
    public class GridStateFitnessCalculator : BaseFitnessCalculator<GridStateGene, EnergyBalanceFitnessparamter>
    {
        private EnergyStorageGeneFitnessCalculator energyStorageFitnessCalculator;
        private GeneratorGeneFitnessCalculator generatorFitnessCalculator;
        private NetworkEnergyImportFitnessCalculator networkImportFitnessCalculator;

        private EnergyBalanceFitnessparamter fitnessParameters;

        public GridStateFitnessCalculator(EnergyBalanceFitnessparamter fitnessParameters)
        {
            this.fitnessParameters = fitnessParameters;

            energyStorageFitnessCalculator = new EnergyStorageGeneFitnessCalculator();
            generatorFitnessCalculator = new GeneratorGeneFitnessCalculator();
            networkImportFitnessCalculator = new NetworkEnergyImportFitnessCalculator();
        }

        public override double Calculate(Chromosome<GridStateGene> chromosome)
        {
            LoadFitnessParameters();

            double fitnessValue = 0;
            float response = 0;

            foreach (var gridStateGene in chromosome.Genes)
            {
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
                    response += gene.ActivePower;
                }
            }

            double networkFitnessValue = networkImportFitnessCalculator.Calculate(fitnessParameters.EnergyDemand, response);

            if (double.IsInfinity(networkFitnessValue))
            {
                return networkFitnessValue;
            }

            fitnessValue += networkFitnessValue;
            chromosome.FitnessValue = fitnessValue;

            return fitnessValue;
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
