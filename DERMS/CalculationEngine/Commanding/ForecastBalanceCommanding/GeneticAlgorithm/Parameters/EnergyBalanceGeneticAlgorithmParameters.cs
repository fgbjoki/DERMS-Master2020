using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Helpers;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Helpers.FitnessParameters;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.ChromosomeCreators;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.FitnessCalculators.EnergyBalance;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.GeneCreators.EnergyBalance;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.Mutators.EnergyBalance;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.Selectors.EnergyBalanceSelectors;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.EnergyBalance.Genes;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Parameters
{
    public class EnergyBalanceGeneticAlgorithmParameters : BaseGeneticAlgorithmParameters<GridStateGene>
    {
        public EnergyBalanceGeneticAlgorithmParameters(BoundaryParameteres boundaryParameters)
        {
            BoundaryParameters = boundaryParameters;

            EnergyStorageActivePowerCalculation esAPCalculator = new EnergyStorageActivePowerCalculation(boundaryParameters);
            GridStateFitnessCalculator gridStateFitnessCalculator = new GridStateFitnessCalculator(BoundaryParameters);

            ManipulatorWrapper = new ManipulatorsWrapper<GridStateGene>()
            {
                ChromosomeSelector = new EnergyBalanceSelector(gridStateFitnessCalculator),
                Mutator = new GridStateGeneMutator(esAPCalculator) { SimulationInterval = boundaryParameters.SimulationInterval },
                GeneCreator = new GridStateChromosomeCreator(new GridStateGeneCreator(esAPCalculator) { SimulationInterval = boundaryParameters.SimulationInterval })
            };
        }

        public BoundaryParameteres BoundaryParameters { get; set; }
    }
}
