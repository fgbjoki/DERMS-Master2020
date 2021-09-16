using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Fitness.Calculators.Genes;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model.Genes;
using Common.AbstractModel;
using Common.Logger;
using System.Collections.Generic;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Fitness.Calculators.Chromosomes
{
    public class EnergyBalanceFitnessCalculator
    {
        private Dictionary<DMSType, IDERGeneFitnessCalculator> geneCalculators;
        private EnergyImportFitnessCalculator importCalculator;

        public EnergyBalanceFitnessCalculator()
        {
            IDERGeneFitnessCalculator generatorGeneFitnessCalculator = new GeneratorGeneFitnessCalculator();
            geneCalculators = new Dictionary<DMSType, IDERGeneFitnessCalculator>()
            {
                { DMSType.SOLARGENERATOR, generatorGeneFitnessCalculator },
                { DMSType.WINDGENERATOR, generatorGeneFitnessCalculator },
                { DMSType.ENERGYSTORAGE, new EnergyStorageGeneFitnessCalculator() }
            };

            importCalculator = new EnergyImportFitnessCalculator();
        }

        public void Calculate<T>(Chromosome<T> chromosome, DomainParameters domainParameters) where T : DERGene
        {
            float fitnessValue = 0;
            float response = 0;

            foreach (var gene in chromosome.Genes)
            {
                IDERGeneFitnessCalculator calculator;
                if (!geneCalculators.TryGetValue(gene.DMSType, out calculator))
                {
                    Logger.Instance.Log($"[{GetType().Name}] Cannot find calculator for DMSType: {gene.DMSType}");
                    continue;
                }

                fitnessValue += calculator.Calculate(gene, domainParameters);
                response += gene.ActivePower * domainParameters.SimulationInterval / 3600;
            }

            float importedEnergyCost = importCalculator.Calculate(domainParameters, response);
            fitnessValue += importedEnergyCost;

            chromosome.FitnessValue = fitnessValue;
            chromosome.ImportedEnergy = importedEnergyCost / domainParameters.FitnessParameters.CostOfEnergyImportPerKWH;
        }
    }
}
